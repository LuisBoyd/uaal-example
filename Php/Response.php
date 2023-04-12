<?php

class Response{
    public bool $Success;
    public string $Message;

    function __construct(string $Message, bool $Success){
        $this->Success = $Success;
        $this->Message = $Message;
    }

}