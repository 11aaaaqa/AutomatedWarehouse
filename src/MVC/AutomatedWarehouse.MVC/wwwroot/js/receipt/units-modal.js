const unitsOpenModalBtn = document.getElementById('units-open-modal-btn');
const unitsModal = document.getElementById('units-modal');
const selectedUnitsBlock = document.getElementById('selected-units-block');

function calculateUnitsModalRect() {
    const rect = unitsOpenModalBtn.getBoundingClientRect();
    unitsModal.style.top = rect.bottom + 2 + 'px';
    unitsModal.style.left = rect.left + 'px';
    unitsModal.style.width = rect.width + 'px';
}

function handleClickOutsideUnitModal(event) {
    if (!unitsModal.contains(event.target) && !selectedUnitsBlock.contains(event.target)
        && !document.getElementById('units-dropdown-block').contains(event.target) && event.target !== unitsOpenModalBtn) {
        unitsModal.style.display = 'none';
    }
}

unitsOpenModalBtn.addEventListener('click', function () {
    if (unitsModal.style.display === 'none' || unitsModal.style.display === '') {
        unitsModal.style.display = 'block';
        calculateUnitsModalRect();
        document.addEventListener('click', handleClickOutsideUnitModal);
    } else {
        unitsModal.style.display = 'none';
        document.removeEventListener('click', handleClickOutsideUnitModal);
    }
});

function selectUnit(element) {
    const unitId = element.getAttribute('unitId');
    const option = document.querySelector(`option[value="${unitId}"]`);

    if (element.getAttribute('isSelected') === 'false') {
        element.setAttribute('isSelected', 'true');
        element.style.backgroundColor = '#ccc';

        option.selected = true;

        if (selectedUnitsBlock.childElementCount === 0) {
            selectedUnitsBlock.textContent = '';
        }

        const selectedUnitSpan = document.createElement('span');
        selectedUnitSpan.setAttribute('unitId', unitId);
        selectedUnitSpan.textContent = element.textContent;
        selectedUnitSpan.style.backgroundColor = '#ccc';
        selectedUnitSpan.style.margin = '0 7px 5px 0';
        selectedUnitSpan.style.padding = '0 2px';
        selectedUnitSpan.style.borderRadius = '4px';
        selectedUnitsBlock.appendChild(selectedUnitSpan);
    }
    else if (element.getAttribute('isSelected') === 'true') {
        element.setAttribute('isSelected', 'false');
        element.style.backgroundColor = 'transparent';

        option.selected = false;

        const elementToDelete = document.querySelector(`span[unitId="${unitId}"]`);
        elementToDelete.remove();

        if (selectedUnitsBlock.childElementCount === 0) {
            selectedUnitsBlock.textContent = 'Выберите';
        }
    }
    calculateUnitsModalRect();
}