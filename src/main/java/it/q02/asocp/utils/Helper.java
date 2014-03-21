package it.q02.asocp.utils;

import java.util.Collection;

/**
 *
 */
public class Helper {

    public static boolean isNullOrEmpty(String object){
        return object == null || object.isEmpty();
    }

    public static boolean isNullOrEmpty(Collection object){
        return object == null || object.isEmpty();
    }
}
