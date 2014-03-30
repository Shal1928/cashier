package it.q02.asocp.modules.administrator.server.rpc;

import com.google.gwt.user.server.rpc.RemoteServiceServlet;
import it.q02.asocp.database.AttractionDAO;
import it.q02.asocp.database.DataBaseService;
import it.q02.asocp.modules.administrator.client.rpc.AttractionService;
import it.q02.asocp.modules.base.client.data.Attraction;
import it.q02.asocp.users.ExecutionContextStorage;

import java.util.List;
import java.util.UUID;

/**
 * User: aleksander at  29.03.14, 20:43
 */
public class AttractionServiceImpl extends RemoteServiceServlet implements AttractionService {

    @Override
    public Attraction saveAndUpdate(Attraction attraction) {
        DataBaseService service = ExecutionContextStorage.getContext().getDatabaseService();
        AttractionDAO dao =service.getMapper(AttractionDAO.class);
        attraction.setOwner(ExecutionContextStorage.getContext().getUserInfo().getUserLogin());
        if(attraction.getVersionSeries() == null){
            attraction.setVersionSeries(UUID.randomUUID().toString());
            dao.create(attraction);
        }else{
            if(attraction.hasSales()){
                Attraction newAttraction = new Attraction();
                newAttraction.cloneFrom(attraction);
                dao.create(newAttraction);
            }else{
                dao.update(attraction);
            }
        }
        service.commitTransaction();
        return attraction;
    }


    @Override
    public List<Attraction> find() {
        return getDAO().find();
    }


    private AttractionDAO getDAO() {
        DataBaseService service = ExecutionContextStorage.getContext().getDatabaseService();
        return service.getMapper(AttractionDAO.class);
    }

}