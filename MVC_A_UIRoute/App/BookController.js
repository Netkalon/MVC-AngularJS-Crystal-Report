

uiroute.controller('BookController', function ($scope, BookService, $window, $http) {
    
    $scope.loadTable = function () {
        var Param={
            Mode:'GET'
        }
        var ServiceData = BookService.loadGrid(Param);
        ServiceData.then(function (result) {
            $scope.LoadData = result.data;
        }, function () {
        });
    }

    $scope.loadTable();

    $scope.LoadById = function (model)

    {
        $scope.Ed = model;
        $scope.Datamode = 'Update';
        $scope.Enable = true;
    }

    $scope.Save = function ()
    {
        if ($scope.Datamode === 'Update')
        {
            var Param = {
                Mode: 'EDIT',
                BookCode: $scope.Ed.BookCode,
                BookName: $scope.Ed.BookName,
                BookDesc: $scope.Ed.BookDesc,
                BookAuthor: $scope.Ed.BookAuthor
            }
        }
        
        else
        {
            var Param = {
                Mode: 'ADD',
                BookCode: $scope.Ed.BookCode,
                BookName: $scope.Ed.BookName,
                BookDesc: $scope.Ed.BookDesc,
                BookAuthor: $scope.Ed.BookAuthor
            }
        }
        var ServiceData = BookService.EditData(Param);
        ServiceData.then(function (result) {
            $scope.loadTable()
            $scope.Ed = '';         
            $scope.message = "Data Save Successfully";
        }, function () {

        });
    }

    $scope.DaleteById = function (model) {
        var Param = {
            Mode: 'DELETE',
            BookCode: model.BookCode,
        }
        var ServiceData = BookService.EditData(Param);
        ServiceData.then(function (result) {
            $scope.loadTable()
            $scope.Ed = '';
            $scope.message = "Data Delete Successfully";
        }, function () {

        });

    }

    $scope.Clear=function()
    {
        $scope.Ed = '';
        $scope.Enable = false;
        $scope.Datamode = '';
    }

    $scope.ExcelReport=function()
    {
        $window.open("Home/ExportExcel", "_blank");
    }
    $scope.PdfReport = function () {
        $window.open("Home/ExportPdf", "_blank");
    }

});

uiroute.service('BookService', function ($http, $window) {

    this.loadGrid = function (Param) {
        var response = $http({
            method: "post",
            url: "Home/LoadData",
            data: JSON.stringify(Param),
            dataType: "json"
        });
        return response;
    }

    this.EditData = function (Param) {
        var response = $http({
            method: "post",
            url: "Home/EditData",
            data: JSON.stringify(Param),
            dataType: "json"
        });
        return response;
    }

    //this.ReportData = function () {
    //    debugger
    //    var response = $http({
    //        method: "get",
    //        url: "Home/ExportExcel",
    //        dataType: "json"
    //    });
    //    return response;
    //}


});

