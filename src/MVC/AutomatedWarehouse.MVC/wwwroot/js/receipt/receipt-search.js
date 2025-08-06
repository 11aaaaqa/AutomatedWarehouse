const applyFiltersBtn = document.getElementById('apply-filters-btn');
const receiptNumbersSelect = document.getElementById('receipt-numbers-select');
const resourcesSelect = document.getElementById('resources-select');
const unitsSelect = document.getElementById('units-select');
const infoWaitingDataBlock = document.getElementById('info-waiting-data-block');

applyFiltersBtn.addEventListener('click', async function () {
  document.getElementById('receipt-documents-table').remove();

  infoWaitingDataBlock.style.display = 'block';

  const dateFrom = document.getElementById('date-from-input').value;
  const dateUntil = document.getElementById('date-until-input').value;
  const selectedReceiptNumbers = Array.from(receiptNumbersSelect.selectedOptions).map(option => option.value);
  const selectedResourceIds = Array.from(resourcesSelect.selectedOptions).map(option => option.value);
  const selectedUnitIds = Array.from(unitsSelect.selectedOptions).map(option => option.value);
  const data = {
    DateFrom: dateFrom, DateUntil: dateUntil, ReceiptNumbers: selectedReceiptNumbers,
    ResourceIds: selectedResourceIds, MeasurementUnitIds: selectedUnitIds
  };

  const response = await fetch('receipts/filter/get-json',
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });

  const responseData = await response.json();

  fillFilteredDocuments(responseData);
  setUpdateDocumentLinks();
  infoWaitingDataBlock.style.display = 'none';
});

function fillFilteredDocuments(collectionData) {
  const table = document.createElement('table');
  table.id = 'receipt-documents-table';

  const headerRow = document.createElement('tr');
  const headers = ['Номер', 'Дата', 'Ресурс', 'Единица измерения', 'Количество'];
  headers.forEach(headerText => {
    const th = document.createElement('th');
    th.innerText = headerText;
    headerRow.appendChild(th);
  });
  table.appendChild(headerRow);

  collectionData.forEach(receiptDocument => {
    if (receiptDocument.receiptResources.length === 0) {
      const tr = document.createElement('tr');
      tr.classList.add('tr-link');
      tr.setAttribute('receiptDocumentId', receiptDocument.id);

      const receiptNumberTd = document.createElement('td');
      receiptNumberTd.innerText = receiptDocument.receiptNumber;
      tr.appendChild(receiptNumberTd);

      const dateTd = document.createElement('td');
      dateTd.innerText = new Date(receiptDocument.receiptDate).toLocaleDateString();
      tr.appendChild(dateTd);

      const resourceTd = document.createElement('td');
      tr.appendChild(resourceTd);

      const unitTd = document.createElement('td');
      tr.appendChild(unitTd);

      const quantityTd = document.createElement('td');
      quantityTd.innerText = '0';
      tr.appendChild(quantityTd);

      table.appendChild(tr);
    } else {
      for (let i = 0; i < receiptDocument.receiptResources.length; i++) {
        if (i === 0) {
          const tr = document.createElement('tr');
          tr.classList.add('tr-link');
          tr.setAttribute('receiptDocumentId', receiptDocument.id);

          const receiptNumberTd = document.createElement('td');
          receiptNumberTd.innerText = receiptDocument.receiptNumber;
          receiptNumberTd.rowSpan = receiptDocument.receiptResources.length;
          tr.appendChild(receiptNumberTd);

          const receiptDateTd = document.createElement('td');
          receiptDateTd.innerText = new Date(receiptDocument.receiptDate).toLocaleDateString();
          receiptDateTd.rowSpan = receiptDocument.receiptResources.length;
          tr.appendChild(receiptDateTd);

          const resourceNameTd = document.createElement('td');
          resourceNameTd.innerText = receiptDocument.receiptResources[i].resource.name;
          tr.appendChild(resourceNameTd);

          const unitNameTd = document.createElement('td');
          unitNameTd.innerText = receiptDocument.receiptResources[i].measurementUnit.name;
          tr.appendChild(unitNameTd);

          const quantityTd = document.createElement('td');
          quantityTd.innerText = receiptDocument.receiptResources[i].quantity;
          tr.appendChild(quantityTd);

          table.appendChild(tr);
        } else {
          const tr = document.createElement('tr');
          tr.classList.add('tr-link');
          tr.setAttribute('receiptDocumentId', receiptDocument.id);

          const resourceNameTd = document.createElement('td');
          resourceNameTd.innerText = receiptDocument.receiptResources[i].resource.name;
          tr.appendChild(resourceNameTd);

          const unitNameTd = document.createElement('td');
          unitNameTd.innerText = receiptDocument.receiptResources[i].measurementUnit.name;
          tr.appendChild(unitNameTd);

          const quantityTd = document.createElement('td');
          quantityTd.innerText = receiptDocument.receiptResources[i].quantity;
          tr.appendChild(quantityTd);

          table.appendChild(tr);
        }
      }
    }
  });

  document.getElementById('table-block').appendChild(table);
}