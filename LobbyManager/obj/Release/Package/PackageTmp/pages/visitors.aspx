<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="visitors.aspx.cs" Inherits="VisitorsEnrollment.pages.visitors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">

        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Registro de Visitantes</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Formulario para Visitantes
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <h3>Datos Personales</h3>
                                <form role="form">
                                    <div class="form-group">
                                        <label>Nombres</label>
                                        <input class="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Apellidos</label>
                                        <input class="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label>Empresa</label>
                                        <input class="form-control" />
                                        <p class="help-block">Si es particular, dejar vacío.</p>
                                    </div>
                                    <div class="form-group">
                                        <label>Teléfono de Contacto</label>
                                        <input class="form-control" placeholder="(503)2222-2222" />
                                    </div>
                                    <div class="form-group">
                                        <label>Fecha de Ingreso</label>
                                        <p class="form-control-static"><%= DateTime.Now.ToLocalTime() %></p>
                                    </div>
                                </form>
                            </div>
                            <!-- /.col-lg-6 (nested) -->
                            <div class="col-lg-6">
                                <h3>Detalle de la Visita</h3>
                                <form role="form">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="reasonSelect">Motivo</label>
                                            <select id="reasonSelect" class="form-control" onchange="alert('Visita Programada');">
                                                <option>Recepción de Correspondencia</option>
                                                <option>Recepción de Factura</option>
                                                <option>Entrega de Cheque</option>
                                                <option>Visita Programada</option>
                                                <option>Visita No Programada</option>
                                            </select>
                                        </div>
                                        <div class="form-group">
                                            <label for="descriptionText">Descripción</label>
                                            <input class="form-control" id="descriptionText" type="text" placeholder="Objetivo de la visita" />
                                        </div>
                                        <div class="form-group">
                                            <label for="deptSelect">Departamento</label>
                                            <select id="deptSelect" class="form-control">
                                                <option>No especificado</option>
                                                <option>Recepción</option>
                                                <option>Dirección General</option>
                                                <option>Ventas y mercadeo</option>
                                                <option>Contabilidad</option>
                                                <option>Compras e Importaciones</option>
                                                <option>Informática</option>
                                                <option>Soporte Técnico</option>
                                                <option>Mantenimiento</option>
                                            </select>
                                        </div>
                                        <div class="form-group">
                                            <label for="contactText">Contacto</label>
                                            <input class="form-control" id="contactText" type="text" placeholder="Nombre del contacto interno" />
                                        </div>
                                        <div class="checkbox">
                                            <label>
                                                <input type="checkbox" />Ingresa equipo
                                            </label>
                                        </div>
                                        <button type="submit" class="btn btn-primary">Aceptar</button>
                                        <button type="reset" class="btn btn-default">Cancelar</button>
                                    </fieldset>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
