package it.q02.asocp.users.helper;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;

import static it.q02.asocp.utils.Helper.isNullOrEmpty;

/**
 *
 */
public class UserRoles extends ArrayList<String> {

    private final String AUTHENTICATED = "AUTHENTICATED";

    public UserRoles(){
        super();
    }

    public UserRoles(String[] collection){
        super();
        Collections.addAll(this, collection);
    }

    public UserRoles(Collection<String> collection){
        super();
        this.addAll(collection);
    }

    public boolean isOnlyAut(){
        return this.size() == 1 && AUTHENTICATED.equals(this.get(0));
    }

    public boolean hasRole(String role){
        for(String r : this)
            if(!isNullOrEmpty(r) && r.equals(role)) return true;

        return false;
    }

    public UserRoles getIgnoreAut(){
        UserRoles result = new UserRoles();
        for (String r : this)
            if(!isNullOrEmpty(r) && !r.equals(AUTHENTICATED)) result.add(r);

        return result;
    }
}
