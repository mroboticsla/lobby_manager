<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Mobile.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="LobbyManager.pages.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        tfoot input {
            width: 100%;
            padding: 3px;
            box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>

        <div class="container">
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div class="login-panel panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Inicio de Sesión</h3>
                        </div>
                        <div class="panel-body">
                            <div class="alert alert-danger" runat="server" visible="false" id="msgError">
                                Usuario o Contraseña no válido.
                            </div>
                            <fieldset>
                                <div class="form-group">
                                    <asp:TextBox runat="server" id="txt_usr" class="form-control" placeholder="Usuario" autofocus />
                                </div>
                                <div class="form-group">
                                    <input runat="server" id="txt_pwd" class="form-control" placeholder="Contraseña" type="password" value="" />
                                </div>
                                <!-- Change this to a button or input when using this as a form -->
                                <asp:Button class="btn btn-lg btn-success btn-block" runat="server" Text="Iniciar sesión" ID="btn_login" OnClick="DoLogin"/>
                            </fieldset>
                            <fieldset>
                                <div class="form-group">
                                    <asp:Label runat="server" ID="txt_device" Text="N/A" CssClass="pull-right" ForeColor="Gray" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"></asp:SqlDataSource>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">

</asp:Content>
