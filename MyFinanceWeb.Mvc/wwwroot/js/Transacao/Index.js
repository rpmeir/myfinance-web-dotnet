    function Novo() {
        window.location.href = '/Transacao/Cadastrar';
    }

    function Atualizar(id) {
        // Implement the logic to handle the "Editar" action
        window.location.href = '/Transacao/Cadastrar/' + id;
    }

    function Excluir(id) {
        if (!confirm('Deseja excluir este item?')) {
            return;
        }

        fetch('/Transacao/Excluir/' + id, {
            method: 'DELETE'
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Falha ao excluir o item.');
                }

                window.location.reload();
            })
            .catch(function (error) {
                alert(error.message);
            });
    }