<%@ Page MasterPageFile="~/Master.Master" Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="SoftwareDevelopmentTest.Test.Results" %>

<asp:Content ContentPlaceHolderID="ContentMain" runat="server">
    <asp:Label ID="labelPageHeader" runat="server" CssClass="HeaderText">Your Score: %QUESTIONSCORRECT%/15 (%PERCENTAGE%)</asp:Label>
    <br /><br />
    <asp:Label ID="labelUnit1Header" CssClass="SubHeaderText" runat="server">Unit 1 - Core Programming</asp:Label>
    <hr /><br />
    <asp:Panel ID="panelUnit1Questions" runat="server"></asp:Panel>
    <br /><br />
    <asp:Label ID="labelUnit2Header" CssClass="SubHeaderText" runat="server">Unit 2 - Object Orientated Programming</asp:Label>
    <hr /><br />
    <asp:Panel ID="panelUnit2Questions" runat="server"></asp:Panel>
    <br /><br />
    <asp:Label ID="labelUnit3Header" CssClass="SubHeaderText" runat="server">Unit 3 - General Software Development</asp:Label>
    <hr /><br />
    <asp:Panel ID="panelUnit3Questions" runat="server"></asp:Panel>
    <br /><br />
    <asp:Button ID="buttonRetake" CssClass="PrimaryButton" OnClick="buttonRetake_Click" runat="server" Text="Retake Test" />
    <asp:Button ID="buttonHome" CssClass="SecondaryButton" OnClick="buttonHome_Click" runat="server" Text="Home ⌂" />

</asp:Content>