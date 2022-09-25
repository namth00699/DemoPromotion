$(document).ready(
    $(function () {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 50) {
                $('#navbar_top').addClass('fixed-top');
                // add padding top to show content behind navbar
                navbar_height = $('.navbar').offsetHeight;
                document.body.style.paddingTop = navbar_height + 'px';
            } else {
                $('#navbar_top').removeClass('fixed-top');
                // remove padding top from body
                document.body.style.paddingTop = '0';
            }
        });
    })
);

document.addEventListener("DOMContentLoaded", function () {
    window.addEventListener('scroll', function () {
        if (window.scrollY > 50) {
            document.getElementById('navbar_top').classList.add('fixed-top');
            // add padding top to show content behind navbar
            navbar_height = document.querySelector('.navbar').offsetHeight;
            document.body.style.paddingTop = navbar_height + 'px';
        } else {
            document.getElementById('navbar_top').classList.remove('fixed-top');
            // remove padding top from body
            document.body.style.paddingTop = '0';
        }
    });
});

$(document).ready(
    $(function () {
        $('.category-select').click(function () {
            var self = $(this);
            var url = '/api/content/GetProductByQuery';
            var category = self[0].dataset.categoryCode;
            $('#category').val(category);
            var index = $('#pageIndex').val();
            var size = $('#pageSize').val();
            var campaign = $('#campaignName').val();
            var sort = $('#sorting-product').find(":selected").val();;

            $.ajax({
                url: url,
                data: { pageIndex: index, pageSize: size, category: category, campaign: campaign, sort: sort },
                type: 'Post',
                success: function (response) {
                    $('.product-containter').remove();
                    $('#outer-containter-product').append(response.html);
                    if (!response.hasMore) {
                        $('#showMore').hide();
                        $('#pageIndex').val(1);
                    }
                    else {
                        $('#showMore').show();
                    }
                }
            });
        });
    })
);

$(document).ready(
    $(function () {
        $('#showMore').click(function () {
            var self = $(this);
            var url = '/api/content/GetProductByQuery';
            var category = $('#category').val();
            var index = parseInt($('#pageIndex').val()) + 1;
            var size = $('#pageSize').val();
            var campaign = $('#campaignName').val();
            var sort = $('#sorting-product').find(":selected").val();;

            $.ajax({
                url: url,
                data: { pageIndex: index, pageSize: size, category: category, campaign: campaign, sort: sort },
                type: 'Post',
                success: function (response) {
                    $('#pageIndex').val(index);
                    //$('#product-containter').remove();
                    $('#outer-containter-product').append(response.html);
                    if (!response.hasMore) {
                        $('#showMore').hide();
                        $('#pageIndex').val(1);
                    }
                    else {
                        $('#showMore').show();
                    }
                }
            });
        });
    })
);

$(document).ready(
    $(function () {
        $('#sorting-product').change(function () {
            var self = $(this);
            var url = '/api/content/GetProductByQuery';
            var category = $('#category').val();
            var index = $('#pageIndex').val();
            var size = $('#pageSize').val();
            var campaign = $('#campaignName').val();
            var sort = self.find(":selected").val();;

            $.ajax({
                url: url,
                data: { pageIndex: index, pageSize: size, category: category, campaign: campaign, sort: sort },
                type: 'Post',
                success: function (response) {
                    $('.product-containter').remove();
                    $('#outer-containter-product').append(response.html);
                    if (!response.hasMore) {
                        $('#showMore').hide();
                        $('#pageIndex').val(1);
                    }
                    else {
                        $('#showMore').show();
                    }
                }
            });
        });
    })
);