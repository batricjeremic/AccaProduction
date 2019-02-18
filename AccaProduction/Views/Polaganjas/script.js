function getSelectedAttribute() {
    var sl = document.getElementById("firstAttribute");
    return sl.options[sl.selectedIndex].text;
}
function changeSearchProperty() {
    var rb1 = document.getElementById("option1");
    var rb2 = document.getElementById("option2");
    var rb3 = document.getElementById("option3");

    var lb1 = document.getElementById("lbl1");
    var lb2 = document.getElementById("lbl2");
    var lb3 = document.getElementById("lbl3");

    if (getSelectedAttribute() == "Ispit") {
        rb1.value = "Name"; lb1.innerHTML = lb1.innerHTML.replace("Ime", "Naziv ispita");
        rb2.value = "OldCode"; lb2.innerHTML = lb2.innerHTML.replace("Email", "Stara oznaka");
        rb3.value = "NewCode"; lb3.innerHTML = lb3.innerHTML.replace("Odeljenje", "Nova oznaka");
    }
    if (getSelectedAttribute() == "Kandidat") {
        rb1.value = "Ime"; lb1.innerHTML = lb1.innerHTML = lb1.innerHTML.replace("Naziv ispita", "Ime");
        rb2.value = "Email"; lb2.innerHTML = lb2.innerHTML.replace("Stara oznaka", "Email");
        rb3.value = "Odeljenje"; lb3.innerHTML = lb3.innerHTML.replace("Nova oznaka", "Odeljenje");
    }
}