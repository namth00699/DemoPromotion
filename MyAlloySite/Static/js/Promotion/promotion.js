

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

$(document).ready(
    $(function () {
        $('#listCategory').change(function () {
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