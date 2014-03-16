package it.q02.asocp.users;

import it.q02.asocp.database.DataBaseService;
import it.q02.asocp.users.impl.ExecutionContextImpl;

/**
 * User: aleksander at  16.03.14, 12:36
 */
public class ExecutionContextFactory {

    public ExecutionContext createContext(UserInfo info,DataBaseService service){
        return new ExecutionContextImpl(info,service);
    }

}
