(function () {
    var deleteLinkObj;
    var item_name;
    var str, start_pos, end_pos, type, text;

    // Delete Link Click Function
    $('.delete-link').click(function () {
        deleteLinkObj = $(this);  //for future use

        // what do you want to delete?
        str = deleteLinkObj[0].pathname;
        start_pos = str.indexOf('/') + 1;
        end_pos = str.indexOf('/', start_pos);
        type = str.substring(start_pos, end_pos)

        item_name = $(this).parent().parent().children('td:first-child').text();
        $("#deleteModal .item-name").text("the " + type + " " + item_name);

        $('#deleteModal').modal('show');
        return false; // prevents the default behaviour
    });

    $('.deleteitem').click(function () {
        $.post(deleteLinkObj[0].href, function (data) {  //Post to action
            if (data == 'True') {
                $('#deleteModal').modal('hide');
                //deleteLinkObj.closest("tr").hide('fast'); //Hide Row
                deleteLinkObj.closest("tr").fadeOut(500);
            }
            else {
                str = deleteLinkObj[0].pathname;
                start_pos = str.indexOf('/') + 1;
                end_pos = str.indexOf('/', start_pos);
                type = str.substring(start_pos, end_pos)

                switch (type)
                {
                    case "region": text = "You can't delete the region, because there are addressess in the region.";
                        break;
                    case "category": text = "You can't delete the category, because there are entities in the category.";
                        break;
                    case "role": text = "You can't delete the role, because some users have this role.";
                        break;
                    case "address": text = "You can't delete the address, because a user has this address.";
                        break;
                    default: text = "Not possible to delete!";
                        break;
                }
                $('#deleteModal').modal('hide');

                $('.delete-errors').text(text);
                $('.delete-errors').show();
            }
        });
    });

    $('.nav-header').on('click', function (e) {
        if ($(this).children(':last-child').hasClass('glyphicon-chevron-right')) {
            $(this).children(':last-child').removeClass('glyphicon-chevron-right');
            $(this).children(':last-child').addClass('glyphicon-chevron-down');
        }
        else {
            $(this).children(':last-child').removeClass('glyphicon-chevron-down');
            $(this).children(':last-child').addClass('glyphicon-chevron-right');
        }
    });

    $('td').on("click", ".favorite", function () {
        console.log("favorite");
        var object = $(this);
        var entityid = object.parent().attr('id');
        // MAKE AJAX CALL 
        $.ajax({
            type: 'POST',
            url: '/favorites/addtofavorites',
            contentType: 'application/json; charset=utf-8',
            data: "{ 'entityID':" + entityid + "}",
            dataType: "json",
            success: function (data) {
                console.log(data.success);
                object.removeClass('favorite');
                object.addClass('unfavorite');
            },
            error: function () {
                alert("error");
            }
        });

    });

    $('td').on("click", ".unfavorite", function () {
        console.log("unfavorite");
        var object = $(this);
        var entityid = object.parent().attr('id');
        // MAKE AJAX CALL 
        $.ajax({
            type: 'POST',
            url: '/favorites/deletefromfavorites',
            contentType: 'application/json; charset=utf-8',
            data: "{ 'entityID':" + entityid + "}",
            dataType: "json",
            success: function (data) {
                console.log(data.success);
                object.removeClass('unfavorite');
                object.addClass('favorite');
            },
            error: function () {
                alert("error");
            }
        });

    });

})();