function SelectOrUnselectAll(gridviewId, obj, targetObj, deleteButtonObj) {
    //this function decides whether to check or uncheck all
    var TargetBaseControl =
            document.getElementById(gridviewId);
    var TargetChildControl = targetObj;

    var TargetChildDeleteControl = deleteButtonObj;

    //Get all the control of the type INPUT in the base control.
    var Inputs = TargetBaseControl.getElementsByTagName("input");

    //Checked/Unchecked all the checkBoxes in side the GridView.
    for (var n = 0; n < Inputs.length; ++n) {
        if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].disabled == false)
            Inputs[n].checked = obj.checked;

        if (Inputs[n].type == 'image' && Inputs[n].id.indexOf(TargetChildDeleteControl, 0) >= 0)
            Inputs[n].disabled = !obj.checked;
    }
}

function UpdateSelectAllAndDeleteControl(gridviewID, targetObj, selectAllCheckBox, deleteBtn) {
    var TotalChk = 0;
    var Counter = 0;

    var TargetBaseControl = document.getElementById(gridviewID);
    TotalChk = TargetBaseControl.rows.length - 2;

    var TargetChildSelectAllControl = document.getElementById(selectAllCheckBox);
    var TargetChildDeleteControl = deleteBtn;
    var Inputs = TargetBaseControl.getElementsByTagName("input");

    Counter = CountCheckedCheckBox(gridviewID, targetObj);

    for (var n = 0; n < Inputs.length; ++n) {
        if (Inputs[n].type == 'image' && Inputs[n].id.indexOf(TargetChildDeleteControl, 0) >= 0)
            if (Counter > 0)
                Inputs[n].disabled = false;
            else
                Inputs[n].disabled = true;
    }

    if (Counter >= TotalChk)
        TargetChildSelectAllControl.checked = true;
    else
        TargetChildSelectAllControl.checked = false;
}


function CountCheckedCheckBox(gridviewID, targetObj) {

    // Added as ASPX uses SPAN for checkbox
    var countChecked = 0;
    var TargetBaseControl =
            document.getElementById(gridviewID);
    var TargetChildControl = targetObj;

    //Get all the control of the type INPUT in the base control.
    var Inputs = TargetBaseControl.getElementsByTagName("input");

    //Checked/Unchecked all the checkBoxes in side the GridView.
    for (var n = 0; n < Inputs.length; ++n)
        if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked)
            countChecked++;

    return countChecked;
}

function DeleteConfirmation(gridviewID, targetObj) {
    var countDelete = CountCheckedCheckBox(gridviewID, targetObj);
    if (confirm("Are you sure, you want to delete " + countDelete + " selected records ?") == true)
        return true;
    else
        return false;
}


//For initialization of google map
var geocoder;
var directionDisplay;
var directionsService; 
var map;

function Initialize(address) {
    geocoder = new google.maps.Geocoder();
    directionsDisplay = new google.maps.DirectionsRenderer();
    directionsService = new google.maps.DirectionsService(); 
    codeAddress(address);

    var latlng = new google.maps.LatLng(0,0);
    var myOptions = {
        zoom: 16,
        scrollwheel: false,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var tempMap = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    map = tempMap;

    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById("DirectionsPanel"));
    $("#HyperLinkViewLargeMap").hide();
}

function codeAddress(address) {
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({ map: map, position: results[0].geometry.location });
        }
        else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}

function CalculateRoute(destinationAddress) {
    var originAddress = document.getElementById("txtFromAddress").value;
    var selectedMode = document.getElementById("ddlMode").value;
    var request = {
        origin: originAddress,
        destination: destinationAddress,
        // Note that Javascript allows us to access the constant       
        // using square brackets and a string value as its       
        // "property."       
        travelMode: google.maps.TravelMode[selectedMode.toUpperCase()]
    };
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            $("#HyperLinkViewLargeMap").show();
            $("#HyperLinkViewLargeMap").attr('href', 'http://maps.google.com/maps?saddr=' + originAddress + '&daddr=' + destinationAddress);
            directionsDisplay.setDirections(response);
        }
    });
}
