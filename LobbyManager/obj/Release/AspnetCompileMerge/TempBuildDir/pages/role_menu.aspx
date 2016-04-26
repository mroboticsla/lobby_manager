<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="role_menu.aspx.cs" Inherits="LobbyManager.pages.role_menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <table>
                    <tr>
                        <td>
                            <button type="button" class="btn btn-primary btn-lg" onclick="window.history.back();"><i class="fa fa-arrow-circle-o-left fa-2x"></i></button>
                        </td>
                        <td>
                            <h1 class="page-header" style="margin-left: 10px;">Opciones de Menu - <asp:Label runat="server" id="lblRoleTitle" Text=""></asp:Label></h1>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:Label ID="lblTitle" runat="server">Usuarios Registrados en el Sistema</asp:Label>
                    </div>
                    <div class="panel-body">
                        <table class="table table-striped table-bordered table-hover" id="equipmentDataTable">
                            <thead>
                                <tr>
                                    <th class="info h4" rowspan="2" style="vertical-align: middle;">Opción de Menú</th>
                                    <th class="info h4" colspan="3" style="text-align: center;">Nivel de Permiso</th>
                                </tr>
                                <tr>
                                    <th class="success" style="text-align: center;">Control total</th>
                                    <th class="warning" style="text-align: center;">Solo consulta</th>
                                    <th class="danger" style="text-align: center;">Ninguno</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                            <ItemTemplate>
                                                <tr class="gradeU <%# (DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim().Equals(DataBinder.Eval(Container.DataItem, "menu_root").ToString().Trim()))?"info" : "" %>">
                                                    <td class="<%# (DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim().Equals(DataBinder.Eval(Container.DataItem, "menu_root").ToString().Trim()))?"h4" : "" %>"><%# DataBinder.Eval(Container.DataItem, "menu_label").ToString().Trim() %></td>
                                                    <td style="text-align: center;"><input class="roleAccess <%# (DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim().Equals(DataBinder.Eval(Container.DataItem, "menu_root").ToString().Trim()))?"hidden" : "" %>" type="radio" name="rad_<%# DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim() + "_" + Request.QueryString["role"] %>" value="0" <%# (DataBinder.Eval(Container.DataItem, "role_access").ToString().Trim().Equals("0"))?"checked=\"checked\"" : "" %> /></td>
                                                    <td style="text-align: center;"><input class="roleAccess <%# (DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim().Equals(DataBinder.Eval(Container.DataItem, "menu_root").ToString().Trim()))?"hidden" : "" %>" type="radio" name="rad_<%# DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim() + "_" + Request.QueryString["role"] %>" value="1" <%# (DataBinder.Eval(Container.DataItem, "role_access").ToString().Trim().Equals("1"))?"checked=\"checked\"" : "" %> /></td>
                                                    <td style="text-align: center;"><input class="roleAccess <%# (DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim().Equals(DataBinder.Eval(Container.DataItem, "menu_root").ToString().Trim()))?"hidden" : "" %>" type="radio" name="rad_<%# DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim() + "_" + Request.QueryString["role"] %>" value="2" <%# (DataBinder.Eval(Container.DataItem, "role_access").ToString().Trim().Equals("2"))?"checked=\"checked\"" : "" %> /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <asp:SqlDataSource ID="SqlDataSourceList" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
    <!-- DataTables JavaScript -->
    <script src="../bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="../bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        function deleteRecord() {
            PageMethods.deleteRecord(currentrecord, OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                //alert(response);
                hideMsg();
                window.location.reload();
            }
        }

        $(document).ready(function () {
            $(".roleAccess").change(function () {
                var values = this.name.split('_');
                
                var param = { role_id: values[2], menu_id: values[1], role_access: this.value };
                $.ajax({
                    url: "role_menu.aspx/SetMenuOption",
                    data: JSON.stringify(param),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        //Datos guardados
                        //alert('Datos guardados!');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
                
            });
        });
    </script>
</asp:Content>
