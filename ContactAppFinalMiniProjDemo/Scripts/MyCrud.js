//function loadContacts() {
//    $.ajax({
//        url: "/Contact/GetContacts",
//        type: "GET",
//        success: function (data) {

//            console.log(data);
//            $("#contactsTable").empty();

//            if (data.length > 0) {
//                $.each(data, function (index, contact) {
//                    var row = `<tr>
//                                 <td>${contact.FName}</td>
//                                 <td>${contact.LName}</td>
//                                /*<td>${contact.IsActive}</td>*/

//                                  <td>
//                                    <input type="checkbox" class="is-active-checkbox"
//                                           data-contactid="${contact.Id}"
//                                           ${contact.IsActive ? "checked" : ""} />
//                                 </td>

                                 

//                                  <td>
//                                 <button onClick="editContact(${contact.Id}) value="Edit" class="btn btn-success" id="#btnEdit">Edit</button>
//                                 </td>

                     
//                                  <td>
//                                 <button onClick="getContactDetails(${contact.Id}) value="Contact Details" class="btn btn-success">getContactDetails</button>
//                                 </td>

//                                 </tr>`;
//                    $("#contactsTable").append(row);
//                });

//                // Add event listener for all checkboxes
//                $(".is-active-checkbox").change(function () {
//                    var contactId = $(this).data("contactid");
//                    var isActive = $(this).is(":checked");

//                    // Call the server to update the contact status
//                    updateContactStatus(contactId, isActive);
//                });


//            } else {
//                $("#contactsTable").append("<tr><td colspan='4'>No contacts found</td></tr>");
//            }
//        },
//        error: function (err) {
//            console.log("Error fetching contacts:", err);
//        }
//    });
//}




function addNewRecord() {
    // Collect data from form fields
    var newContact = {
        FName: $("#Fname").val(),
        LName: $("#Lname").val()
        /*IsActive: $("#IsActive").is(":checked")*/
    };

    $.ajax({
        url: "/Contact/Add", // Ensure this matches your route correctly
        type: "POST",
        data: JSON.stringify(newContact), // Ensure data is serialized as JSON
        contentType: "application/json; charset=utf-8", // Set content type
        success: function (response) {
            alert("New contact added successfully");
            loadContacts(); // Refresh the contact list
            $("#newRecord").hide(); // Hide the add form
            $("#contactList").show(); // Show the contacts table
        },
        error: function (err) {
            alert("Error adding new contact");
            console.log(err);
        }
    });
}




function loadContacts() {
    $.ajax({
        url: "/Contact/GetContacts",
        type: "GET",
        success: function (data) {
            console.log(data);
            $("#contactsTable").empty();

            if (data.length > 0) {
                $.each(data, function (index, contact) {
                    var row = `<tr>
                                 <td>${contact.FName}</td>
                                 <td>${contact.LName}</td>
                                 <td>
                                    <input type="checkbox" class="is-active-checkbox"
                                           data-contactid="${contact.Id}"
                                           ${contact.IsActive ? "checked" : ""} />
                                 </td>
                                 <td>
                                    <button onClick="editContact('${contact.Id}')" class="btn btn-success">Edit</button>
                                 </td>
                                 <td>
                                    <button onClick="getContactDetails('${contact.Id}')" class="btn btn-primary">Contact Details</button>
                                 </td>
                               </tr>`;
                    $("#contactsTable").append(row);
                });

                // Add event listener for all checkboxes
                $(".is-active-checkbox").change(function () {
                    var contactId = $(this).data("contactid");
                    var isActive = $(this).is(":checked");

                    // Call the server to update the contact status
                    updateContactStatus(contactId, isActive);
                });
            } else {
                $("#contactsTable").append("<tr><td colspan='4'>No contacts found</td></tr>");
            }
        },
        error: function (err) {
            console.log("Error fetching contacts:", err);
        }
    });
}

function getContact(contactId) {
    $.ajax({
        url: "/Contact/GetContactById",
        type: "GET",
        data: { id: contactId },
        success: function (response) {
            if (response.success) {
                $("#editContactId").val(response.contact.Id);
                $("#newFName").val(response.contact.FirstName);
                $("#newLName").val(response.contact.LastName);
            } else {
                alert(response.message);
            }
        },
        error: function (err) {
            alert("No such data found");
        }
    });
}

function modifyRecord(modifiedContact) {
    $.ajax({
        url: "/Contact/EditContact",
        type: "POST",
        data: modifiedContact,
        success: function (response) {
            if (response.success) {
                alert("Contact Edited Successfully");
                loadContacts();
                $("#contactList").show();
                $("#editContact").hide();
            } else {
                alert(response.message);
            }
        },
        error: function (err) {
            alert("Error Editing Record");
        }
    });
}


// Function to send AJAX request to update contact status
function updateContactStatus(contactId, isActive) {
    $.ajax({
        url: "/Contact/UpdateContactStatus",
        type: "POST",
        data: JSON.stringify({ contactId: contactId, isActive: isActive }),
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                alert("Contact status updated successfully");
            } else {
                alert("Error updating contact status: " + response.message);
            }
        },
        error: function (err) {
            console.log("Error updating contact status:", err);
        }
    });
}



// Show the "Add New Contact" form when the "Add New" button is clicked
$("#btnAdd").click(function () {
    $("#contactList").hide(); // Hide contact list
    $("#newRecord").show(); // Show add form
});

function editContact(contactId) {
    getContact(contactId);
    $("#contactList").hide();
    $("#editRecord").show();
}

$("#btnEdit").click(() => {
    var data = {
        Id: $("#editContactId").val(),
        FName: $("#newFName").val(),
        LName: $("#newLName").val(),
    };
    modifyRecord(data);
});




//////  CONTACT DETAILS ////////

function getContactDetails(contactId) {
    window.location.href = `/ContactDetails/Index?contactId=${contactId}`
}