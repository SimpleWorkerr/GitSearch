from flask import Flask, jsonify, request
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.cluster import KMeans

app = Flask(__name__)

def analyze_similarity_kmeans(user_input_name, user_input_description, items):
    # Объединение названия и описания для каждого элемента
    combined_data = [user_input_name + " " + user_input_description] + \
                    [item.full_name + " " + item.description for item in items]
    vectorizer = TfidfVectorizer(stop_words='english')
    X = vectorizer.fit_transform(combined_data)

    # KMeans кластеризация
    n_clusters = 2  # Выбор количества кластеров
    model = KMeans(n_clusters=n_clusters, init='k-means++', max_iter=100, n_init=1)
    model.fit(X)

    # Определение кластера для каждого элемента
    clusters = model.predict(X)
    return clusters

@app.route('/analyze', methods=['POST'])
def analyze_repositories():
    data = request.json
    user_input_name = data['user_input_name']  # Название, введенное пользователем
    user_input_description = data['user_input_description']  # Описание, введенное пользователем
    items = data['fullResult'];

    result = items

    # Сопоставление элементов с их кластерами
    # for i, item in enumerate(items):
    #     result.append({            
    #         'full_name': item.full_name,
    #         'description': item.description,
    #     })

    return jsonify(items)

if __name__ == '__main__':
    app.run(debug=True)
