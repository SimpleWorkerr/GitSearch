document.addEventListener("DOMContentLoaded", function () {
  var themeToggleButton = document.getElementById("theme-toggle");
  // Загрузить сохранённую тему из localStorage
  var savedTheme = localStorage.getItem("theme") || "light"; // Светлая тема по умолчанию
  document.documentElement.setAttribute("data-theme", savedTheme);

  themeToggleButton.addEventListener("click", function () {
    var currentTheme = document.documentElement.getAttribute("data-theme");
    var newTheme = currentTheme === "dark" ? "light" : "dark";
    document.documentElement.setAttribute("data-theme", newTheme);
    // Сохранить выбранную тему в localStorage
    localStorage.setItem("theme", newTheme);
  });
});
