import { Button, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap'
import { useState } from 'react';
import axios from 'axios';


export default function EditEntry( { activities, reload, setReload, setLoading, entry, totalSum, isFrozen, setError }){
    const [subactivities, setSubactivities] = useState(entry.activity.subactivities);
    const [selectedActivity, setSelectedActivity] = useState(entry.activity);
    const [selectedSubactivity, setSelectedSubactivity] = useState(entry.subactivity);
    const [time, setTime] = useState(entry.time);
    const [description, setDescription] = useState(entry.description);
    const [show, setShow] = useState(false);

    const handleClose = () => {
        setShow(false);
        setSubactivities(entry.activity.subactivities);
        setSelectedActivity(entry.activity);
        setSelectedSubactivity(entry.subactivity);
        setTime(entry.time);
        setDescription(entry.description);
    }
    const handleShow = () => setShow(true);

    const handleCodeChange = (e) => {
        const selected = JSON.parse(e.target.value);
        setSelectedActivity(selected);
        setSubactivities(selected.subactivities);
        e.preventDefault();
    }

    const handleSubcodeChange = (e) => {
        if(e.target.value === ''){
            setSelectedSubactivity(e.target.value);
        }
        else{
            setSelectedSubactivity(JSON.parse(e.target.value));
        }
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
        axios.put('api/entries/' + entry.entryId, 
            {
                EntryId: entry.entryId,
                Date: entry.date,
                ActivityId: selectedActivity.activityId,
                SubactivityId: selectedSubactivity === '' || selectedSubactivity === null ? null : selectedSubactivity.subactivityId,
                Time: time,
                description: description,
                WorkerId: entry.workerId,
                ReportId: entry.reportId

            })
        .then(response => {
            if(response.status === 204){
                setLoading(true);
                setReload(!reload);
            }
        }).catch(error => {
            let msg = error.message;
            if(error.response){
                msg = error.response.data;
            }
            setError(msg);
        })
            
        handleClose();
        e.preventDefault();
    }

    const calculateSelectedCode = (activity) => {
        if(selectedActivity.activityId === activity.activityId){
            return  <option key={activity.activityId} value={JSON.stringify(activity)} selected>{activity.code}</option>
        }
        return <option key={activity.activityId} value={JSON.stringify(activity)}>{activity.code}</option>
    }

    const calculateSelectedSubcode = (subactivity) => {
        if(selectedSubactivity !== null && subactivity.subactivityId === selectedSubactivity.subactivityId){
            return <option key={subactivity.subactivityId} value={JSON.stringify(subactivity)} selected>{subactivity.code}</option>;
        }
        return <option key={subactivity.subactivityId} value={JSON.stringify(subactivity)}>{subactivity.code}</option>;
    }

    const modalButton = !isFrozen
        ? 
        <Button variant="success" onClick={handleShow}>
            Edit
        </Button>
        :
        <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">This month is frozen</Tooltip>}>
            <span className="d-inline-block">
                <Button variant="success" disabled>
                    Edit
                </Button>
            </span>
        </OverlayTrigger>;

    return (
        <div className='col-sm-2'>
            {modalButton}

            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Edit Entry</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                <form id='add-entry-form' onSubmit={handleSubmit}>
                    <div className='form-group mb-3 row'>
                        <label htmlFor="activityId" className="col-sm-3 col-form-label">Project</label>
                        <div className='col'>
                            <select className="form-select" onChange={handleCodeChange} required>
                                {activities.map(activity =>
                                    calculateSelectedCode(activity)
                                )}
                            </select>  
                        </div>
                    </div>
                    <div className="form-group mb-3 row">
                        <label htmlFor="subcode" className="col-sm-3 col-form-label">Subactivity</label>
                        <div className='col'>
                            <select className="form-select" onChange={handleSubcodeChange}>
                                <option value=''>----------</option>
                                {subactivities.map(subactivity => 
                                    calculateSelectedSubcode(subactivity)
                                )}
                            </select>
                        </div>             
                    </div>
                    <div className="form-group mb-3 row">
                        <label htmlFor="time" className="col-sm-3 col-form-label">Time</label>
                        <div className='col'>
                            <input type="number" name='time' className="form-control" min="1" max={500 - (totalSum - entry.time)} value={time} onChange={handleTimeChange} required />         
                        </div>     
                    </div>
                    <div className="form-group mb-3 row">
                        <label htmlFor="description" className="col-sm-3 col-form-label">Description</label>
                        <div className='col'>
                            <textarea name='description' className="form-control" rows="1" maxLength="255" value={description} onChange={handleDescriptionChange} />
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