const resourcesOpenModalBtn = document.getElementById('resources-open-modal-btn');
const resourcesModal = document.getElementById('resources-modal');
const selectedResourcesBlock = document.getElementById('selected-resources-block');

function calculateResourcesModalRect() {
    const rect = resourcesOpenModalBtn.getBoundingClientRect();
    resourcesModal.style.top = rect.bottom + 2 + 'px';
    resourcesModal.style.left = rect.left + 'px';
    resourcesModal.style.width = rect.width + 'px';
}

function handleClickOutsideResourcesModal(event) {
    if (!resourcesModal.contains(event.target) && !selectedResourcesBlock.contains(event.target)
        && !document.getElementById('resources-dropdown-block').contains(event.target) && event.target !== resourcesOpenModalBtn) {
        resourcesModal.style.display = 'none';
    }
}

resourcesOpenModalBtn.addEventListener('click', function () {
    if (resourcesModal.style.display === 'none' || resourcesModal.style.display === '') {
        resourcesModal.style.display = 'block';
        calculateResourcesModalRect();
        document.addEventListener('click', handleClickOutsideResourcesModal);
    } else {
        resourcesModal.style.display = 'none';
        document.removeEventListener('click', handleClickOutsideResourcesModal);
    }
});

function selectResource(element) {
    const resourceId = element.getAttribute('resourceId');
    const option = document.querySelector(`option[value="${resourceId}"]`);

    if (element.getAttribute('isSelected') === 'false') {
        element.setAttribute('isSelected', 'true');
        element.style.backgroundColor = '#ccc';

        option.selected = true;

        if (selectedResourcesBlock.childElementCount === 0) {
            selectedResourcesBlock.textContent = '';
        }

        const selectedResourceSpan = document.createElement('span');
        selectedResourceSpan.setAttribute('resourceId', resourceId);
        selectedResourceSpan.textContent = element.textContent;
        selectedResourceSpan.style.backgroundColor = '#ccc';
        selectedResourceSpan.style.margin = '0 7px 5px 0';
        selectedResourceSpan.style.padding = '0 2px';
        selectedResourceSpan.style.borderRadius = '4px';
        selectedResourcesBlock.appendChild(selectedResourceSpan);
    }
    else if (element.getAttribute('isSelected') === 'true') {
        element.setAttribute('isSelected', 'false');
        element.style.backgroundColor = 'transparent';

        option.selected = false;

        const elementToDelete = document.querySelector(`span[resourceId="${resourceId}"]`);
        elementToDelete.remove();

        if (selectedResourcesBlock.childElementCount === 0) {
            selectedResourcesBlock.textContent = 'Выберите';
        }
    }
    calculateResourcesModalRect();
}