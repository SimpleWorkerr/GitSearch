document.addEventListener("DOMContentLoaded", function () {
  var menuButton = document.getElementById("menuButton");
  var nav = document.getElementById("nav");
  var menuOverlay = document.getElementById("menuOverlay");

  function toggleMenu() {
    if (menuOverlay.style.opacity === "1") {
      menuOverlay.style.opacity = "0";
      setTimeout(function () {
        menuOverlay.style.display = "none";
      }, 300); // Задержка соответствует продолжительности transition
      nav.style.right = "-250px";
    }
    
    else {
      menuOverlay.style.display = "block";
      setTimeout(function () {
        menuOverlay.style.opacity = "1";
      }, 10); // Небольшая задержка перед началом анимации прозрачности
      nav.style.right = "0px";
    }
  }

  menuButton.addEventListener("click", toggleMenu);

  menuOverlay.addEventListener("click", toggleMenu);

  // Закрытие меню при нажатии клавиши Esc
  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
      toggleMenu();
    }
  });
});
