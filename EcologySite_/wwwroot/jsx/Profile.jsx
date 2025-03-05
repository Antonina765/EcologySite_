import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ProfileLayout from '/Profile_Layout';

const Profile = () => {
    const [userName, setUserName] = useState('');
    const [avatarUrl, setAvatarUrl] = useState('');
    const [posts, setPosts] = useState([]);
    const [comments, setComments] = useState([]);
    const [avatarFile, setAvatarFile] = useState(null);

    useEffect(() => {
        axios.get('~/Controllers/EcologyController/Profile')
            .then(response => {
                const data = response.data;
                setUserName(data.userName);
                setAvatarUrl(data.avatarUrl);
                setPosts(data.posts);
                setComments(data.comments);
            })
            .catch(error => {
                console.error('Error fetching profile data:', error);
            });
    }, []);

    const handleAvatarChange = (e) => {
        setAvatarFile(e.target.files[0]);
    };

    const handleAvatarUpdate = (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append('avatar', avatarFile);
        axios.post('~/Controllers/EcologyController/UpdateAvatar', formData)
            .then(response => {
                setAvatarUrl(response.data.avatarUrl);
            })
            .catch(error => {
                console.error('Error updating avatar:', error);
            });
    };

    return (
        <ProfileLayout userName={userName}>
            <div className="profile">
                <div>
                    <h1>{userName}</h1>
                </div>
                <div>
                    Avatar:
                    <img className="avatar" src={avatarUrl} alt="User Avatar" />
                </div>
                <div>
                    <form onSubmit={handleAvatarUpdate}>
                        <input type="file" name="avatar" onChange={handleAvatarChange} />
                        <button type="submit">Update avatar</button>
                    </form>
                </div>
                <h3>Your Posts</h3>
                {posts.length > 0 ? (
                    <ul>
                        {posts.map(post => (
                            <li key={post.id}>
                                <div className="post">
                                    <img src={post.imageSrc} alt="Post Image" className="post-image" />
                                    <p>{post.texts}</p>
                                </div>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <p>You have not created any posts yet.</p>
                )}
                <h3>Your Comments</h3>
                {comments.length > 0 ? (
                    <ul>
                        {comments.map(comment => (
                            <li key={comment.id}>
                                <div className="comment">
                                    <p>{comment.commentText}</p>
                                </div>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <p>You have not left any comments yet.</p>
                )}
            </div>
        </ProfileLayout>
    );
};

export default Profile;
