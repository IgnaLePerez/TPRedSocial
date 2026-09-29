const inputContraseña = document.getElementById("contraseña");
const msjContraseña = document.getElementById("msjContraseña");

function verificarContraseña(){
    console.log("entró")
    let contraseñaIngresada = document.getElementById("contraseña").value;

    if (contraseñaIngresada.length < 8){
        inputContraseña.style.borderColor = "red";
        msjContraseña.innerHTML = "La contraseña debe tener como mínimo 8 caracteres";
        msjContraseña.style.color = "red";
        return false
    }
    return true
}