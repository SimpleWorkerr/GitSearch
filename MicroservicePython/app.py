from flask import Flask, jsonify, request
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity

app = Flask(__name__)

def split_user_input(user_input):
    # Разбиваем запрос пользователя на компоненты
    split_components = user_input.replace("-", " ").split()
    return [component.lower() for component in split_components]

def calculate_similarity(vectorizer, user_text, repo_text):
    # Рассчитываем косинусное сходство между текстами
    vect_texts = vectorizer.transform([user_text, repo_text])
    similarity = cosine_similarity(vect_texts[0], vect_texts[1])
    return similarity[0][0]

def analyze_repositories(user_input_name, user_input_description, items):
    key_words = split_user_input(user_input_description)  # Ключевые слова из описания
    user_combined_text = user_input_name + " " + user_input_description
    vectorizer = TfidfVectorizer(stop_words='english')
    vectorizer.fit([user_combined_text] + [item.get('description') or '' for item in items])

    result = []
    for item in items:
        repo_description = item.get('description') or ''
        if any(key_word in repo_description.lower() for key_word in key_words):
            description_similarity = calculate_similarity(vectorizer, user_combined_text, repo_description)
            if description_similarity > 0.05:  # Сниженный порог схожести
                result.append(item)

    return result

@app.route('/analyze', methods=['POST'])
def analyze():
    data = request.json
    user_input_name = data['user_input_name']
    user_input_description = data['user_input_description']
    items = data['fullResult']

    analysis_result = analyze_repositories(user_input_name, user_input_description, items)

    print(items)
    return jsonify(analysis_result)

if __name__ == '__main__':
    app.run(debug=True)