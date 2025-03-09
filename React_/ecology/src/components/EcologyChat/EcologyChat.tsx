import { useCallback, useEffect, useState } from "react";
import { IEcologyPost } from "../../models/IEcologyPost";
import "./EcologyChat.css";
import { CreateEcologyPost, EcologyPost } from "..";
import { apiEcologyChat } from "../../api.services";

function EcologyChat() {
    const [posts, setEcologyPosts] = useState<IEcologyPost[]>([]); // ~view models

    useEffect(() => {
        apiEcologyChat.GetEcologyPosts().then(setEcologyPosts);
    }, []);

    const getMaxEcologyPostId = useCallback(() => {
        const ids = posts.map((p) => p.id);
        return Math.max(...ids);
    }, [posts]);
    
    const createEcologyPost = useCallback((ecology: IEcologyPost) => {
        apiEcologyChat.CreatePost(ecology);
        setEcologyPosts((oldPosts) => [...oldPosts, ecology]);
    }, []);

    const onDeletePost = useCallback((id: number) => {
        setEcologyPosts((oldPosts) => oldPosts.filter((p) => p.id != id));
    }, []);
    
    return (
        <div>
            <h1>Ecology Posts</h1>

            <div className="girls">
                {posts.map((post) => (
                    <EcologyPost key={post.id} post={post} onDelete={onDeletePost}></EcologyPost>
                ))}
                <CreateEcologyPost getMaxEcologyPostId={getMaxEcologyPostId} createPost={createEcologyPost} />
            </div>
        </div>
    );
}

export { EcologyChat };
