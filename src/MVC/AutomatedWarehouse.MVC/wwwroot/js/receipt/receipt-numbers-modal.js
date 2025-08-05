const receiptNumberOpenModalBtn = document.getElementById('receipt-numbers-open-modal-btn');
const receiptNumbersModal = document.getElementById('receipt-numbers-modal');
const selectedReceiptNumbersBlock = document.getElementById('selected-receipt-numbers-block');

function calculateNumbersModalRect() {
    const rect = receiptNumberOpenModalBtn.getBoundingClientRect();
    receiptNumbersModal.style.top = rect.bottom + 2 + 'px';
    receiptNumbersModal.style.left = rect.left + 'px';
    receiptNumbersModal.style.width = rect.width + 'px';
}

function handleClickOutsideNumberModal(event) {
    if (!receiptNumbersModal.contains(event.target) && !selectedReceiptNumbersBlock.contains(event.target)
        && !document.getElementById('receipt-numbers-dropdown-block').contains(event.target) && event.target !== receiptNumberOpenModalBtn) {
        receiptNumbersModal.style.display = 'none';
    }
}

receiptNumberOpenModalBtn.addEventListener('click', function () {
    if (receiptNumbersModal.style.display === 'none' || receiptNumbersModal.style.display === '') {
        receiptNumbersModal.style.display = 'block';
        calculateNumbersModalRect();
        document.addEventListener('click', handleClickOutsideNumberModal);
    } else {
        receiptNumbersModal.style.display = 'none';
        document.removeEventListener('click', handleClickOutsideNumberModal);
    }
});

function selectReceiptNumber(element) {
    const option = document.querySelector(`option[value="${element.textContent}"]`);

    if (element.getAttribute('isSelected') === 'false') {
        element.setAttribute('isSelected', 'true');
        element.style.backgroundColor = '#ccc';

        option.selected = true;

        if (selectedReceiptNumbersBlock.childElementCount === 0) {
            selectedReceiptNumbersBlock.textContent = '';
        }

        const selectedReceiptNumberSpan = document.createElement('span');
        selectedReceiptNumberSpan.setAttribute('numberValue', element.textContent);
        selectedReceiptNumberSpan.textContent = element.textContent;
        selectedReceiptNumberSpan.style.backgroundColor = '#ccc';
        selectedReceiptNumberSpan.style.margin = '0 7px 5px 0';
        selectedReceiptNumberSpan.style.padding = '0 2px';
        selectedReceiptNumberSpan.style.borderRadius = '4px';
        selectedReceiptNumbersBlock.appendChild(selectedReceiptNumberSpan);
    }
    else if (element.getAttribute('isSelected') === 'true') {
        element.setAttribute('isSelected', 'false');
        element.style.backgroundColor = 'transparent';

        option.selected = false;

        const elementToDelete = document.querySelector(`span[numberValue="${element.textContent}"]`);
        elementToDelete.remove();

        if (selectedReceiptNumbersBlock.childElementCount === 0) {
            selectedReceiptNumbersBlock.textContent = 'Выберите';
        }
    }
    calculateNumbersModalRect();
}