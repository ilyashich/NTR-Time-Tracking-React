import { useState, useEffect, useContext } from "react";
import { useHistory } from 'react-router-dom';
import axios from 'axios';
import FormData from 'form-data';
import UserContext from '../UserContext';

export default function Login() {
  
    const [workers, setWorkers] = useState([]);
    const [selectedWorker, setSelectedWorker] = useState('');
    const [loading, setLoading] = useState(true);
    const [newUser, setNewUser] = useState('');

    const { setSessionWorker, setIsLoggedIn} = useContext(UserContext);

    const history = useHistory();

    

    useEffect(() => {
        
        const populateEntriesData = async () => {
          const response = await axios.get('api/workers');
          setWorkers(response.data);
          setLoading(false);
        }
        
        populateEntriesData();
    }, []);

    const handleSubmit = (e) => {
        axios.get('api/workers/session/'+ selectedWorker)
        .then(response => {
          setSessionWorker(response.data);
          setIsLoggedIn(true);
          history.replace("/home")
        });
        e.preventDefault();
    }
    const handleChange = (e) => {
        setSelectedWorker(e.target.value);
        e.preventDefault();
    }

    const handleUserAdd = (e) => {
        const form_data = new FormData();
        form_data.append('name', newUser);
        axios.post('api/workers', form_data).then(response => {
          setSessionWorker(response.data);
          setIsLoggedIn(true);
          history.replace("/home")
        });
        e.preventDefault();
    }

    const handleNameChange = (e) => {
        setNewUser(e.target.value);
        e.preventDefault();
    }

    const renderLoginSelect= (workers) => {
      return (
        <form onSubmit={handleSubmit}>
          <div className="d-flex justify-content-center">
            <div className="form-group row">
              <select className="form-select" id="login-select" onChange={handleChange}>
                <option hidden>Select name</option>
                  {workers.map(worker =>
                  <option key={worker.workerId} value={worker.workerId}>{worker.name}</option>
                  )}
              </select>               
            </div>
          </div>
            <div className="d-flex justify-content-center">
              <input type="submit" className="btn btn-primary" value="Login" />
            </div>
        </form>
      );
    }

    let contents = loading
      ? <div> 
          <div className="text-center">
            <h1 className="card-title placeholder-glow">
              <span className="placeholder col-1"></span>
            </h1>
          </div>
          <div className="d-flex justify-content-center">
            <input id="placeholder-button" className="btn btn-primary disabled placeholder" aria-hidden="true"></input>
          </div>
        </div>
      : renderLoginSelect(workers);

    return(
      <div>
        <div className="text-center">
            <p className="h3">Login</p>
        </div>
        {contents}
        <div id="add-new-user" className="text-center">
            <p className="h5">Add new user</p>
        </div>
        <form onSubmit={handleUserAdd}>
          <div className="d-flex justify-content-center">
            <div className="form-group row">
              <input type="text" id="new-user-input" className="form-control" placeholder="Enter Name" onChange={handleNameChange} />              
            </div>
          </div>
            <div className="d-flex justify-content-center">
              <input type="submit" className="btn btn-primary" value="Login" />
            </div>
        </form>
      </div>
    );
  
}
