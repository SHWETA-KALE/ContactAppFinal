$(document).ready(function () {
    $("#grid").jqGrid({
        url: "/contactDetails/GetContactDetails",
        datatype: "json",
        colNames: ["Id", "PhoneNumber", "Email"],
        colModel: [{ name: "Id", key: true, hidden: true },
            { name: "PhoneNumber", editable: true, searchoptions: { sopt: ['eq'] } },
            { name: "Email", editable: true, search: false }],
        height: "250",
        caption: "Contact Details",
        pager: "#pager",
        rowNum: 5,
        rowList: [5, 10, 15],
        sortname: 'id',
        sortorder: 'asc',
        viewrecords: true,
        width: "650",

        gridComplete: function () {
            $("#grid").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true, search:true, refresh: true },
                {
                    //edit
                    url: "/ContactDetails/Edit",
                    closeAfterEdit: true,
                    width: 600,

                    afterSubmit: function (response, postdata) {
                        var result = JSON.parse(response.responseText);
                        if (result.success) {
                            alert(result.message);
                            return [true];
                        } else {
                            alert(result.message);
                            return [false];
                        }
                    }
                },
                {
                    url: "/ContactDetails/Add",
                    closeAfterAdd: true,
                    width: 600,

                    afterSubmit: function (response, postdata) {
                        var result = JSON.parse(response.responseText);
                        if (result.success) {
                            alert(result.message);
                            return [true];
                        } else {
                            alert(result.message);
                            return [false];
                        }
                    }
                },
                {
                    url: "/ContactDetails/Delete",
                    closeAfterDelete: true,
                    width: 600,

                    afterSubmit: function (response, postdata) {
                        var result = JSON.parse(response.responseText);
                        if (result.success) {
                            alert(result.message);
                            return [true];
                        } else {
                            alert(result.message);
                            return [false];
                        }
                    }
                },
                {
                    multipleSearch: false,
                    closeAfterSearch: true
                }
                
            ); //  jqgrid ends here
           
        }

    })
})