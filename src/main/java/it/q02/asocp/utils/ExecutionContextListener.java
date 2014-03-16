package it.q02.asocp.utils;

import it.q02.asocp.database.DataBaseProvider;
import it.q02.asocp.users.ExecutionContext;
import it.q02.asocp.users.ExecutionContextFactory;
import it.q02.asocp.users.ExecutionContextStorage;
import it.q02.asocp.users.UserInfo;


import javax.servlet.ServletRequest;
import javax.servlet.ServletRequestEvent;
import javax.servlet.ServletRequestListener;
import javax.servlet.http.HttpServletRequest;
import java.util.LinkedList;

/**
 * User: aleksander at  16.03.14, 12:25
 */
public class ExecutionContextListener implements ServletRequestListener {

    public static final String ROLES_LIST_PARAM = "available.roles";

    @Override
    public void requestInitialized(ServletRequestEvent servletRequestEvent) {
        ServletRequest servletRequest = servletRequestEvent.getServletRequest();
        if (servletRequest instanceof HttpServletRequest) {
            UserInfo userInfo = null;
            if(((HttpServletRequest) servletRequest).isUserInRole("AUTHENTICATED")){
                 userInfo = UserInfoProvider.Provider.getUserInfo(((HttpServletRequest) servletRequest).getUserPrincipal().getName());
            }
            ExecutionContext context = new ExecutionContextFactory().createContext(userInfo, DataBaseProvider.createService());
            ExecutionContextStorage.setExecutionContenxt(context);
        }
    }


    @Override
    public void requestDestroyed(ServletRequestEvent servletRequestEvent) {
        try {
            ExecutionContext context = ExecutionContextStorage.getContext();
            if (context != null) {
                context.getDatabaseService().getSession().close();
            }
        } finally {
            ExecutionContextStorage.setExecutionContenxt(null);
        }

    }

}
