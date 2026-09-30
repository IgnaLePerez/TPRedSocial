// Función para cargar comentarios
async function cargarComentarios(idPublicacion) {
    try {
        const response = await fetch(`/Home/ObtenerComentarios?idPublicacion=${idPublicacion}`);
        if (!response.ok) throw new Error('Error en la respuesta del servidor');
        
        const comentarios = await response.json();
        
        const containerModal = document.getElementById(`comentarios-modal-${idPublicacion}`);
        const containerIndex = document.getElementById(`comentarios-${idPublicacion}`);
        
        let html = '';
        if (comentarios.length === 0) {
            html = '<p style="text-align: center; color: #536471; font-size: 13px;">No hay comentarios aún</p>';
        } else {
            html = comentarios.map(c => `
                <div class="comentario-item">
                    <div class="comentario-autor">${c.nombre} ${c.apellido}</div>
                    <div class="comentario-usuario">@${c.nombreUsuario}</div>
                    <div class="comentario-texto">${c.texto}</div>
                    <div class="comentario-fecha">${new Date(c.fechaComentario).toLocaleString('es-ES')}</div>
                </div>
            `).join('');
        }
        
        if (containerModal) containerModal.innerHTML = html;
        if (containerIndex && comentarios.length > 0) {
            containerIndex.innerHTML = `
                <div style="font-size: 12px;">
                    <strong style="color: #0f1419;">@${comentarios[0].nombreUsuario}</strong>
                    <span style="color: #536471;"> ${comentarios[0].texto.substring(0, 50)}${comentarios[0].texto.length > 50 ? '...' : ''}</span>
                </div>
                ${comentarios.length > 1 ? `<small style="color: #536471;">Ver más comentarios (${comentarios.length})</small>` : ''}
            `;
        }
    } catch (error) {
        console.error('Error al cargar comentarios:', error);
    }
}

// Cargar likes al cargar la página
async function cargarLikes() {
    const tarjetas = document.querySelectorAll('.publicacion-card');
    for (const tarjeta of tarjetas) {
        const idPublicacion = tarjeta.dataset.publicacionId;
        if (idPublicacion) {
            cargarComentarios(idPublicacion);
        }
    }
}

// Inicializar cuando el DOM está listo
document.addEventListener('DOMContentLoaded', function() {
    // Cargar likes y comentarios
    cargarLikes();
    
    // Manejador del botón de like
    document.querySelectorAll('.btn-like').forEach(btn => {
        btn.addEventListener('click', async (e) => {
            e.preventDefault();
            const idPublicacion = btn.dataset.publicacionId;
            
            if (!idPublicacion) {
                console.error('ID de publicación no encontrado');
                return;
            }
            
            try {
                const response = await fetch('/Home/TogglearLike', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ idPublicacion: parseInt(idPublicacion) })
                });
                
                if (!response.ok) throw new Error('Error al dar like');
                
                const data = await response.json();
                
                // Actualizar UI
                const likesCount = document.querySelector(`.likes-count-${idPublicacion}`);
                if (likesCount) {
                    likesCount.textContent = data.cantidadLikes;
                }
                
                // Cambiar ícono si el usuario ya hizo like
                if (data.usuarioYaLikeó) {
                    btn.classList.add('liked');
                    btn.innerHTML = '<i class="fas fa-heart"></i>';
                } else {
                    btn.classList.remove('liked');
                    btn.innerHTML = '<i class="far fa-heart"></i>';
                }
            } catch (error) {
                console.error('Error al dar like:', error);
            }
        });
    });

    // Manejador del formulario de comentarios
    document.querySelectorAll('.form-comentario').forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            const idPublicacion = form.dataset.publicacionId;
            const textoInput = form.querySelector('.texto-comentario');
            const texto = textoInput.value.trim();
            
            if (!texto) {
                alert('Por favor escribe un comentario');
                return;
            }
            
            if (!idPublicacion) {
                console.error('ID de publicación no encontrado');
                return;
            }
            
            try {
                const response = await fetch('/Home/AgregarComentario', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ idPublicacion: parseInt(idPublicacion), texto: texto })
                });
                
                if (!response.ok) throw new Error('Error al agregar comentario');
                
                textoInput.value = '';
                cargarComentarios(idPublicacion);
                
                // Cerrar modal después de agregar comentario
                const modal = document.getElementById(`modalComentarios${idPublicacion}`);
                if (modal) {
                    const bootstrapModal = bootstrap.Modal.getInstance(modal);
                    if (bootstrapModal) {
                        bootstrapModal.hide();
                    }
                }
            } catch (error) {
                console.error('Error al agregar comentario:', error);
                alert('Error al agregar comentario');
            }
        });
    });

    // Cargar comentarios al abrir el modal
    document.querySelectorAll('[data-bs-toggle="modal"]').forEach(btn => {
        if (btn.classList.contains('btn-comentario')) {
            btn.addEventListener('click', () => {
                const idPublicacion = btn.dataset.publicacionId;
                if (idPublicacion) {
                    cargarComentarios(idPublicacion);
                }
            });
        }
    });
});
