package it.q02.asocp.modules.base.client.helpers;

import java.util.Collection;

/**
 *
 */
public class CommonHelper {

    public static boolean isNullOrEmpty(String object){
        return object == null || object.isEmpty();
    }

    public static boolean isNullOrEmpty(Collection object){
        return object == null || object.isEmpty();
    }
}
