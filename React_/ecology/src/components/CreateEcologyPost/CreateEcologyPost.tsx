import { FC, useCallback, useEffect, useState } from "react";
import { IEcologyPost } from "../../models/IEcologyPost";
import "./CreateEcologyPost.css";
import { EcologyPost } from "..";

type CreateEcologyPostProps = {
    getMaxEcologyPostId: () => number;
    createPost: (post: IEcologyPost) => void;
};

const CreateEcologyPost: FC<CreateEcologyPostProps> = ({ getMaxEcologyPostId, createPost }) => {
    const [newPostText, setNewPostText] = useState<string>("");
    const [newPostUrl, setNewPostUrl] = useState<string>("");
    const [post, setPost] = useState<IEcologyPost>({} as IEcologyPost);

    const addPost = useCallback(() => {
        const newId = getMaxEcologyPostId() + 1;
        const newPost = {
            likesCount: 0,
            text: newPostText,
            url: newPostUrl,
            id: newId,
        } as IEcologyPost;
        createPost(newPost);
        setNewPostText("");
        setNewPostUrl("");
    }, [createPost, getMaxEcologyPostId, newPostText, newPostUrl]);

    useEffect(() => {
        setPost((oldPost) => {
            return { ...oldPost, name: newPostText, url: newPostUrl };
        });
    }, [newPostText, newPostUrl]);

    return (
        <div className="new-post-creation">
            <EcologyPost post={post} isPreview={true} />
            <div className="new-post-creation-info">
                <input
                    value={newPostText}
                    onChange={(evt) => setNewPostText(evt.currentTarget.value)}
                    placeholder="Text for post"
                ></input>
                <input
                    value={newPostUrl}
                    onChange={(evt) => setNewPostUrl(evt.currentTarget.value)}
                    placeholder="Way for picture"
                ></input>
                <button onClick={addPost}>Add</button>
            </div>
        </div>
    );
};

export { CreateEcologyPost };