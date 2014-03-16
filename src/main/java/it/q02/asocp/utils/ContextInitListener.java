package it.q02.asocp.utils;

import it.q02.asocp.database.DataBaseProvider;
import org.apache.log4j.Logger;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.servlet.ServletContext;
import javax.servlet.ServletContextEvent;
import javax.servlet.ServletContextListener;
import java.io.IOException;

/**
 * User: aleksander at  16.03.14, 11:40
 * Этот класс вызывается в момент первой инициализации приложения
 * в контейнере сервлета, и перед тем как контейнер решает удалить приложение.
 */
public class ContextInitListener implements ServletContextListener {
    private static final Logger logger = Logger.getLogger(ContextInitListener.class);

    @Override
    public void contextInitialized(ServletContextEvent servletContextEvent) {
        logger.info("application: start init...");

        try {
           Context initCtx = new InitialContext();
           Context envCntx = (Context)initCtx.lookup("java:comp/env");
           //1. получаем из контекста сервис получения инфы о пользователях
            UserInfoProvider userInfo = (UserInfoProvider) envCntx.lookup("utils/UserInfo");
            logger.info("user info provider is:"+userInfo.toString());
            UserInfoProvider.Provider.initProvider(userInfo);

            DataBaseProvider provider = (DataBaseProvider) envCntx.lookup("utils/DBProvider");
            logger.info("database provider is:"+provider.toString());
            DataBaseProvider.setInstance(provider);;

            provider.createConnection();
            provider.updateDatabase();

        } catch (NamingException e) {
           logger.error("can`t init jndi context",e);
            throw new RuntimeException("can`t init jndi context",e);
        }


    }

    @Override
    public void contextDestroyed(ServletContextEvent servletContextEvent) {

    }

}
