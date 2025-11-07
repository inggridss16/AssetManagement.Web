// This object holds the category-to-subcategory mapping.
var subcategoryData = {
    "IT Equipment": ["Laptop", "Monitor", "Printer"],
    "Furniture": ["Chair", "Table", "Cabinet"]
};

/**
 * Populates the subcategory dropdown based on the selected category.
 * @param {string} selectedCategory The value from the category dropdown.
 * @param {jQuery} $subcategoryDropdown The jQuery object for the subcategory dropdown.
 */
function updateSubcategoryDropdown(selectedCategory, $subcategoryDropdown) {
    $subcategoryDropdown.empty();

    if (selectedCategory && subcategoryData[selectedCategory]) {
        $subcategoryDropdown.append($('<option>', { value: '', text: '-- Select Subcategory --' }));
        $.each(subcategoryData[selectedCategory], function (index, value) {
            $subcategoryDropdown.append($('<option>', {
                value: value,
                text: value
            }));
        });
    } else {
        $subcategoryDropdown.append($('<option>', { value: '', text: '-- Select Category First --' }));
    }
}

/**
 * Hides the Maintenance tab if the subcategory is a type of furniture.
 */
function toggleMaintenanceTab() {
    var furnitureSubcategories = ["Chair", "Table", "Cabinet"];
    var currentSubcategory = $("#subcategoryDropdown").val();
    var $maintenanceTab = $('#maintenance-tab');

    // Only proceed if the tab exists on the page
    if ($maintenanceTab.length === 0) {
        return;
    }

    if (furnitureSubcategories.indexOf(currentSubcategory) > -1) {
        $maintenanceTab.hide();

        // If the maintenance tab was active, switch to the details tab
        if ($maintenanceTab.hasClass('active')) {
            var tab = new bootstrap.Tab(document.querySelector('#asset-details-tab'));
            tab.show();
        }
    } else {
        $maintenanceTab.show();
    }
}

// This block runs when the DOM is ready.
$(document).ready(function () {
    var $categoryDropdown = $("#categoryDropdown");
    var $subcategoryDropdown = $("#subcategoryDropdown");

    // This event handler is for ALL forms using this script.
    $categoryDropdown.on('change', function () {
        updateSubcategoryDropdown($(this).val(), $subcategoryDropdown);
        // Trigger the change event on the subcategory to ensure chained logic runs.
        $subcategoryDropdown.trigger('change');
    });

    // This event handler will only affect pages that have the #maintenance-tab.
    $subcategoryDropdown.on('change', toggleMaintenanceTab);
});