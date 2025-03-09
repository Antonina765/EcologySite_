import { FC } from "react";
import { IEcologyPost } from "../../models/IEcologyPost";
import "./EcologyPost.css";

type EcologyPostProps = {
    post: IEcologyPost;
    onDelete?: (id: number) => void;
    isPreview?: boolean;
};

const EcologyPost: FC<EcologyPostProps> = ({ post, onDelete, isPreview = false }) => {
    const deletePost = () => {
        onDelete?.(post.id);
    };
    
    return (
        <div className={(isPreview ? "preview" : "") + " post"}>
            <div className="ppost-text">
                {post.text} [{post.likesCount}]
            </div>
            <div className="image-container">
                <img src={post.url}></img>
            </div>
            {onDelete && (
                <div>
                    <button onClick={deletePost}>delete</button>
                </div>
            )}
        </div>
    );
};

export {EcologyPost};