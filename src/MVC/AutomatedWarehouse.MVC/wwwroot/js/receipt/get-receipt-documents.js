function setUpdateDocumentLinks() {
    const trs = document.querySelectorAll('.tr-link');
    trs.forEach(tr => {
        tr.addEventListener('click', function () {
            const receiptDocumentId = tr.getAttribute('receiptDocumentId');
            window.location.href = `/receipts/${receiptDocumentId}/update`;
        });
    });
}

setUpdateDocumentLinks();