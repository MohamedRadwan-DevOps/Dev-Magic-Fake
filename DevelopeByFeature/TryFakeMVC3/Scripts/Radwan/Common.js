//This method will fill any dropdown box with the info from another dropdown, all you have to do set the main variables (paramters) before calling
var dropDownSourceId = ""; 				// "#VendorDropList_Id";
var dropDownDestinationId = ""; 			// "#ProductTypeDropList_Id";
var searchByKey = ""; 					//"vendorId";
var dropDownFillURL = ""; 				// "/DemandRequest/GetAllProductByVendorId"; // I can also refacotr this method and make the URL include the /?searchByKey=SearchByValue and delete this 2 variables
var dropDownFormMethod = ""; 			//"POST";



dropDownSourceId = "#VendorForm_Id";
dropDownDestinationId = "#ProductTypeForm_Id";
searchByKey = "vendorId";
dropDownFillURL = "/ProductType/GetAllProductTypeByVendorId"; // I can also refactor this method and make the URL include the /?searchByKey=SearchByValue and delete this 2 variables
dropDownFormMethod = "POST";
//$('#VendorDropList_Id').change(fillAnotherDropdown);
$(dropDownSourceId).change(fillAnotherDropdown);




function fillAnotherDropdown() {
    var searchByValue = $(dropDownSourceId).attr('value');
    if (searchByValue < 0 || searchByValue == "") {
        return;
    }
    showSmallLoaderToggle();
    var dropDownDestinationOptionSelector = dropDownDestinationId + " option";
    jQuery.ajax({
        type: dropDownFormMethod,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        url: dropDownFillURL + "/?" + searchByKey + "=" + searchByValue,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var notificationText = "Error " + XMLHttpRequest.status;  //This line will change to make the XMLHttpRequest.status return word Error or خطاء according to the language
            //showNotification(notificationText);
        },
        success: function (result) {
            if (result.success) {
                $(dropDownDestinationOptionSelector).each(function (i, option) { $(option).remove(); });
                $.each(result.objects, function (index, item) {
                    $(dropDownDestinationId).get(0).options[$(dropDownDestinationId).get(0).options.length] = new Option(item.Name, item.Id);

                });
                $(dropDownDestinationId).prepend("<option value=''>-- Please Select --</option>");
                $(dropDownDestinationId).val("");
            }
            else {
                //showNotification(result.message);
            }
            showSmallLoaderToggle();
        }
    });
};


function showSmallLoaderToggle() {
    var currentImageClass = $("#smallLoader").attr("class");
    if (currentImageClass == "noneDisplay") {
        $("#smallLoader").attr("class", "smallLoader");
    }
    else {
        $("#smallLoader").attr("class", "noneDisplay");
    }
};