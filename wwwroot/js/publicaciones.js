const contadorLikes = document.getElementById("contadorLikes")

function DarLike(id){
    fetch('/Home/TogglearLike?IdPublicacion=' + id, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    })
    .then(response => {
        console.log(response);
        return response.json();
    })
    .then(data => {
        contadorLikes.innerHTML = data.cantidadLikes;
    })
    .catch((error) => {
        console.error('Error:', error);
    });    
}
