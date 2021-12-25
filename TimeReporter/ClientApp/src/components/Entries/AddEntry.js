import { Button, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap'
import { useState } from 'react';
import axios from 'axios';
import FormData from 'form-data';
import Moment from 'moment';


export default function AddEntry( {activities, selectedDate, reload, setReload, setLoading, totalSum, report, setError } ){
    const [subactivities, setSubactivities] = useState([]);
    const [selectedActivity, setSelectedActivity] = useState(null);
    const [subactivityId, setSubactivityId] = useState('');
    const [time, setTime] = useState('');
    const [description, setDescription] = useState('');
    const [show, setShow] = useState(false);

    const handleClose = () => {
        setShow(false);
        setSubactivities([]);
    }
    const handleShow = () => setShow(true);

    const handleCodeChange = (e) => {
        const selected = JSON.parse(e.target.value);
        setSelectedActivity(selected);
        setSubactivities(selected.subactivities);
        e.preventDefault();
    }

    const handleSubcodeChange = (e) => {
        setSubactivityId(e.target.value);
        e.preventDefault();
    }

    const handleTimeChange = (e) => {
        setTime(e.target.value);
        e.preventDefault();
    }

    const handleDescriptionChange = (e) => {
        setDescription(e.target.value);
        e.preventDefault();
    }

    const handleSubmit = (e) => {
        const form_data = new FormData();
        form_data.append('selectedDate', selectedDate);
        form_data.append('activityId', selectedActivity.activityId);
        form_data.append('subactivityId', subactivityId);
        form_data.append('time', time);
        form_data.append('description', description);
        axios.post('api/entries', form_data).then(response => {
            if(response.status === 201){
                setLoading(true);
                setReload(!reload);
            }
        }).catch(error => {
            let msg = error.message;
            if(error.response){
                msg = error.response.data;
            }
            setError(msg);
        });
        handleClose();
        e.preventDefault();
    }

    const calculateTooltipMessage = () => {
        if(Moment().format('YYYY-MM') !== Moment(selectedDate).format('YYYY-MM')){
            return "You can add entries only to current month";
        }
        else if(report !== null && report.frozen){
            return "This month is frozen";
        }
    }

    const tooltipMessage = calculateTooltipMessage();

    const modalButton = Moment().format('YYYY-MM') === Moment(selectedDate).format('YYYY-MM') && (report === null || !report.frozen)
        ? 
        <Button variant="primary" onClick={handleShow}>
            Add new Entry
        </Button>
        :
        <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">{tooltipMessage}</Tooltip>}>
            <span className="d-inline-block">
                <Button variant="primary" disabled>
                    Add new Entry
                </Button>
            </span>
        </OverlayTrigger>;

    return (
        <div className='col-sm-2'>
            {modalButton}

            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Entry</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                <form id='add-entry-form' onSubmit={handleSubmit}>
                    <div className='form-group mb-3 row'>
                        <label htmlFor="activityId" className="col-sm-3 col-form-label">Project</label>
                        <div className='col'>
                            <select className="form-select" onChange={handleCodeChange} required>
                                <option hidden value="">Select Code</option>
                                {activities.map(activity =>
                                <option key={activity.activityId} value={JSON.stringify(activity)}>{activity.code}</option>
                                )}
                            </select>  
                        </div>
                    </div>
                    <div className="form-group mb-3 row">
                        <label htmlFor="subcode" className="col-sm-3 col-form-label">Subactivity</label>
                        <div className='col'>
                            <select className="form-select" onChange={handleSubcodeChange}>
                                <option value=''>-------</option>
                                {subactivities.map(subactivity =>
                                <option key={subactivity.subactivityId} value={subactivity.subactivityId}>{subactivity.code}</option>
                                )}
                            </select>
                        </div>             
                    </div>
                    <div className="form-group mb-3 row">
                        <label htmlFor="time" className="col-sm-3 col-form-label">Time</label>
                        <div className='col'>
                            <input type="number" name='time' className="form-control" min="1" max={500 - totalSum} placeholder="Enter Time" onChange={handleTimeChange} required />         
                        </div>     
                    </div>
                    <div className="form-group mb-3 row">
                        <label htmlFor="description" className="col-sm-3 col-form-label">Description</label>
                        <div className='col'>
                            <textarea type="text" name='description' className="form-control" placeholder="Enter Description" rows="1" maxLength="255" onChange={handleDescriptionChange} />
                        </div>              
                    </div>
                </form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" form='add-entry-form' type='submit'>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}