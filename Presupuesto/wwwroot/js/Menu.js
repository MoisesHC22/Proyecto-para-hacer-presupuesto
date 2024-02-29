const themeToggle = document.querySelector("[data-theme]");
const sideBar_element = document.querySelector(".sideBar");
const like_element = document.querySelector(".link-like");
themeToggle.addEventListener("click", () => {
    themeToggle.querySelector("i").classList.toggle("fa-toggle-on");
    sideBar_element.classList.toggle("dark");
    document.querySelector("body").classList.toggle("dark");
});

like_element.addEventListener("click", () => {
    like_element.querySelector(".fa-thumbs-up").classList.add("like-anim");
    like_element.querySelector(".fa-thumbs-up").classList.toggle("like-color");

    setTimeout(() => {
        like_element.querySelector(".fa-thumbs-up").classList.remove("like-anim");
    }, 600);
});
