<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" CodeBehind="Page1.aspx.cs" Inherits="SoftwareDevelopmentTest.Test.Page1" %>

<asp:Content ContentPlaceHolderID="ContentMain" runat="server">
    <asp:Label ID="labelPage1Title" CssClass="HeaderText" runat="server">Unit 1 - Core Programming</asp:Label>
    <br /><br />
    <asp:Panel runat="server" ID="panelQuestionsPlaceholder">

    </asp:Panel>
    <br /><br />
    <asp:Panel runat="server" ID="panelButtons">
        <asp:Button runat="server" ID="buttonBack" OnClick="buttonBack_Click" CssClass="PrimaryButton" Text="← Back" />
        <asp:Button runat="server" ID="buttonNext" OnClick="buttonNext_Click" Visible="true" CssClass="SecondaryButton" Text="Next →" />
    </asp:Panel>
</asp:Content>
