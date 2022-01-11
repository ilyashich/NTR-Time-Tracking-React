import { Button, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap'
import axios from 'axios';
import { useState } from 'react';


export default function DeleteEntry( { entryId, setLoading, reload, setReload, isFrozen, setError }){
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const handleDelete = (e) => {
        axios.delete('api/entries/' + entryId)
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
            setLoading(true);
            setReload(!reload);
        });
        e.preventDefault();
    }

    const modalButton = !isFrozen
        ? 
        <Button variant="danger" onClick={handleShow}>
            Delete
        </Button>
        :
        <OverlayTrigger overlay={<Tooltip id="tooltip-disabled">This month is frozen</Tooltip>}>
            <span className="d-inline-block">
                <Button variant="danger" disabled>
                    Delete
                </Button>
            </span>
        </OverlayTrigger>;

    return (
        <div className='col-sm-2'>
        {modalButton}
        <Modal show={show} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Confirm delete</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                You sure want to delete this entry?
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
                <Button variant="danger" onClick={handleDelete}>
                    Delete
                </Button>
            </Modal.Footer>
        </Modal>    
    </div>
    );
}