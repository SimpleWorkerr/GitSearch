document.addEventListener("DOMContentLoaded", () => {
  const searchForm = document.getElementById("search-form");
  searchForm.addEventListener("submit", function (event) {
    event.preventDefault();
    const projectName = document.getElementById("project-name").value;
    const projectDescription = document.getElementById(
      "project-description"
    ).value;
    const apiURL = `https://localhost:7101/getRepoByArgs?description=${encodeURIComponent(
      projectDescription
    )}&name=${encodeURIComponent(projectName)}`;

    fetch(apiURL)
      .then((response) => response.json())
      .then((data) => {
        createCards(data);
      })
      .catch((error) => console.error("Ошибка:", error));
  });
});

function createCards(repos) {
  const cardsContainer = document.querySelector(".cards-container");
  cardsContainer.innerHTML = ""; // Очищаем контейнер перед добавлением новых карточек

  repos.forEach((repo) => {
    const cardElement = createCardElement(repo);
    cardsContainer.appendChild(cardElement);
  });
}

function createCardElement(repo) {
  // Создаем элементы карточки
  const card = document.createElement("div");
  card.className = "card";

  const avatar = document.createElement("img");
  avatar.className = "card__avatar";
  avatar.src = repo.avatar_url;
  avatar.alt = "Аватар";

  const info = document.createElement("div");
  info.className = "card__info";

  const title = document.createElement("h3");
  title.className = "card__title";
  title.textContent = repo.full_name;

  const visibility = document.createElement("p");
  visibility.className = "card__visibility";
  visibility.textContent = `Видимость: ${repo.visibility}`;

  const stack = document.createElement("p");
  stack.className = "card__stack";
  stack.textContent = `Используемый язык: ${repo.language || "Не указан"}`;

  const views = document.createElement("span");
  views.className = "card__views";
  views.textContent = `Количество просмотров: ${repo.watchers_count}`;

  const forks = document.createElement("span");
  forks.className = "card__forks";
  forks.textContent = `Форков: ${repo.forks_count}`;

  const link = document.createElement("a");
  link.className = "card__link";
  link.href = repo.html_url;
  link.textContent = "Репозиторий на GitHub";

  // Собираем карточку
  card.appendChild(avatar);
  card.appendChild(info);
  info.appendChild(title);
  info.appendChild(visibility);
  info.appendChild(stack);

  const stats = document.createElement("p");
  stats.className = "card__stats";
  stats.appendChild(views);
  stats.appendChild(forks);
  info.appendChild(stats);

  info.appendChild(link);

  return card;
}