package it.q02.asocp.users;

import it.q02.asocp.database.DataBaseService;

/**
 * User: aleksander at  16.03.14, 12:20
 * Содержит описание контекста выполнения в рамках текущего запроса
 */
public interface ExecutionContext {

    /**
     * Возвращает информацию о текущем пользователе
     * @return
     */
    public UserInfo getUserInfo();

    /**
     * Возвращает связанный с текущим запросом коннект с СУБД
     * @return
     */
    public DataBaseService getDatabaseService();

    /**
     * Возвращает маппер запросов к СУБД
     * @param tClass
     * @param <T>
     * @return
     */
    public <T> T getMapper(Class<T> tClass);

    public boolean dbIsConnected();

}
