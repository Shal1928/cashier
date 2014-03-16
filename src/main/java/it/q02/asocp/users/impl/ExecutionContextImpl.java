package it.q02.asocp.users.impl;

import it.q02.asocp.database.DataBaseProvider;
import it.q02.asocp.database.DataBaseService;
import it.q02.asocp.users.ExecutionContext;
import it.q02.asocp.users.UserInfo;

/**
 * User: aleksander at  16.03.14, 12:34
 */
public class ExecutionContextImpl implements ExecutionContext {

    private DataBaseProvider provider;
    private UserInfo userInfo;
    private DataBaseService service;


    public ExecutionContextImpl(UserInfo userInfo) {
        this.userInfo = userInfo;
        this.provider = provider;

    }

    @Override
    public UserInfo getUserInfo() {
        return userInfo;
    }

    @Override
    public DataBaseService getDatabaseService() {
        if(service == null){
            service = DataBaseProvider.createService();

        }
        return service;
    }



    @Override
    public <T> T getMapper(Class<T> tClass) {
        return service.getMapper(tClass);
    }

    @Override
    public boolean dbIsConnected() {
        return service == null;
    }
}
