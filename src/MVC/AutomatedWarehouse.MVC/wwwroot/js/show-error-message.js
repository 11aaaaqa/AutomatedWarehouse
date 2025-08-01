function showErrorMessage(message) {
    const errorBlock = document.getElementById('error-block');
    if (errorBlock) {
        errorBlock.innerHTML = message;
        errorBlock.style.display = 'block';

        setTimeout(() => {
            errorBlock.style.display = 'none';
        }, 8000);
    }
}