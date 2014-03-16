package it.q02.asocp.users.impl;

import it.q02.asocp.database.DataBaseService;
import it.q02.asocp.users.ExecutionContext;
import it.q02.asocp.users.UserInfo;

/**
 * User: aleksander at  16.03.14, 12:34
 */
public class ExecutionContextImpl implements ExecutionContext {

    private UserInfo userInfo;
    private DataBaseService service;


    public ExecutionContextImpl(UserInfo userInfo, DataBaseService service) {
        this.userInfo = userInfo;
        this.service = service;
    }

    @Override
    public UserInfo getUserInfo() {
        return userInfo;
    }

    @Override
    public DataBaseService getDatabaseService() {
        return service;
    }

    @Override
    public <T> T getMapper(Class<T> tClass) {
        return service.getMapper(tClass);
    }
}
