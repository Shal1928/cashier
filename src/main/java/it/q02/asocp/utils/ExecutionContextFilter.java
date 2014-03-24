package it.q02.asocp.utils;

import it.q02.asocp.users.ExecutionContext;
import it.q02.asocp.users.ExecutionContextFactory;
import it.q02.asocp.users.ExecutionContextStorage;
import it.q02.asocp.users.UserInfo;


import javax.servlet.*;
import javax.servlet.http.HttpServletRequest;
import java.io.IOException;

/**
 * User: aleksander at  16.03.14, 12:25
 */
public class ExecutionContextFilter implements Filter {




    @Override
    public void init(FilterConfig filterConfig) throws ServletException {
        //To change body of implemented methods use File | Settings | File Templates.
    }

    @Override
    public void doFilter(ServletRequest servletRequest, ServletResponse servletResponse, FilterChain filterChain) throws IOException, ServletException {
        createInfo(servletRequest);
        filterChain.doFilter(servletRequest, servletResponse);
        destroyInfo();
    }

    private void createInfo(ServletRequest servletRequest) {
        if (servletRequest instanceof HttpServletRequest) {
            UserInfo userInfo = null;
            if(((HttpServletRequest) servletRequest).isUserInRole("AUTHENTICATED")){
                userInfo = UserInfoProvider.Provider.getUserInfo(((HttpServletRequest) servletRequest).getUserPrincipal().getName());
            }
            ExecutionContext context = new ExecutionContextFactory().createContext(userInfo);
            ExecutionContextStorage.setExecutionContenxt(context);
        }
    }

    private void destroyInfo() {
        try {
            ExecutionContext context = ExecutionContextStorage.getContext();
            if (context != null) {
                if(context.dbIsConnected()){
                    context.getDatabaseService().commitTransaction();
                    context.getDatabaseService().closeSession();
                }
            }
        } finally {
            ExecutionContextStorage.setExecutionContenxt(null);
        }
    }

    @Override
    public void destroy() {
        //To change body of implemented methods use File | Settings | File Templates.
    }
}
