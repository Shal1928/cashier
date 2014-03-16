package it.q02.asocp.users;

/**
 * User: aleksander at  16.03.14, 12:27
 */
public final class ExecutionContextStorage {

    private static ThreadLocal<ExecutionContext> contextThreadLocal = new ThreadLocal<ExecutionContext>();

    public static void setExecutionContenxt(ExecutionContext context){
            contextThreadLocal.set(context);
    }

    public static ExecutionContext getContext(){
        return contextThreadLocal.get();
    }
}
