import { Button, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap'
import { useState, useEffect } from 'react';
import Moment from 'moment';


export default function Accepteds({ selectedDate, report }){
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    
    const handleShow = () => setShow(true);

    const [projectSums, setProjectSums] = useState({});

    useEffect(() => {
        const calculateSum = () => {
            let sums = {};
            if(report !== null && report.accepteds.length > 0){
                for(let i = 0; i < report.entries.length; i++){
                    if(!sums[report.entries[i].activityId]){
                        sums[report.entries[i].activityId] = report.entries[i].time;
                    }
                    else{
                        sums[report.entries[i].activityId] += report.entries[i].time;
                    }
                }
            }
            setProjectSums(sums);
        }
        calculateSum();
    }, [report]);

    if(report !== null && report.accepteds.length > 0){
        return (
            <div className='col-sm-2'>
                <Button variant="primary" onClick={handleShow}>
                    Accepted Times
                </Button>
    
                <Modal size='lg' show={show} onHide={handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>{Moment(selectedDate).format('MMMM YYYY')} Summary</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <table className='table table-striped' aria-labelledby="tabelLabel">
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Code</th>
                                    <th>Total Submitted Time</th>
                                    <th>Accepted Time</th>
                                    <th>Manager</th>
                                </tr>
                            </thead>
                            <tbody>
                                {report.accepteds.map((accepted, i) =>
                                <tr key={accepted.acceptedId}>
                                    <td>{i+1}</td>
                                    <td>{accepted.activity.code}</td>
                                    <td>{projectSums[accepted.activity.activityId]}</td>
                                    <td>{accepted.time}</td>
                                    <td>{accepted.activity.worker.name}</td>                   
                                </tr>
                                )}
                            </tbody>
                        </table>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>
                            Close
                        </Button>
                    </Modal.Footer>
                </Modal>
            </div>
        );      
    }
    return(
        <div className="col-sm-2">
            <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">You don't have accepted activities in this month</Tooltip>}>
                <span className="d-inline-block">
                    <Button variant="primary" disabled>
                        Accepted Times
                    </Button>
                </span>
            </OverlayTrigger>
        </div>
    );    
}