<%
    if (request.getUserPrincipal() != null) {
        response.sendRedirect("/base/");
    } else {
        response.sendRedirect("/security/");
    }
%>
