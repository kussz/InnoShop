
mainFunc = myProducts;
async function myProducts() {
    await fetchItems(`${productHost}/Product/ForUser/${user.id}`, 'Product');
}
async function pendingProducts() {
    await fetchItems(`${productHost}/Product/PendingForUser/${user.id}`, 'Product');
}
async function boughtProducts() {
    await fetchItems(`${productHost}/Product/BoughtForUser/${user.id}`, 'Product');
}
myBtn = document.getElementById("myProds");
pendingBtn = document.getElementById("pendingProds");
boughtBtn = document.getElementById("boughtProds");
myBtn.addEventListener("click", async function (e) {
    mainFunc = myProducts;
    document.getElementById("blobparent").innerText = "";
    addCreateBtn();
    mainFunc();
})
pendingBtn.addEventListener("click", async function (e) {
    mainFunc = pendingProducts;
    document.getElementById("blobparent").innerText = "";
    mainFunc();
})
boughtBtn.addEventListener("click", async function (e) {
    mainFunc = boughtProducts;
    document.getElementById("blobparent").innerText = "";
    mainFunc();
})
async function addCreateBtn() {
    a= document.createElement("a");
    a.setAttribute("id", "add");
    a.setAttribute("class", "blob blob-add");
    a.setAttribute("href", "/Product/Create");
    h11 = document.createElement("h1");
    h11.innerText = "Добавить товар";
    h12 = document.createElement("h1");
    i = document.createElement("i");
    i.setAttribute("class","fas fa-plus")
    h12.appendChild(i);
    a.appendChild(h11);
    a.appendChild(h12);
    document.getElementById("blobparent").appendChild(a);
}