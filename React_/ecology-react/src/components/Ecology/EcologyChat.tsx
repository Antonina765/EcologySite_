import React, { ChangeEventHandler, useCallback, useState } from "react";
import "./EcologyChat.css";

interface Post {
  id: number;
  imageUrl: string | ArrayBuffer | null;
  text: string;
}

function EcologyChat() {
  const [posts, setPosts] = useState<Post[]>([]);
  const [imageUrl, setImageUrl] = useState<string>("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [text, setText] = useState<string>("");
  const [isPostFormVisible, setPostFormVisible] = useState<boolean>(false);
  const [isEditMode, setIsEditMode] = useState<number | null>(null);
  const [activeMenu, setActiveMenu] = useState<number | null>(null);

  const togglePostForm = useCallback(() => {
    setPostFormVisible(!isPostFormVisible);
  }, [isPostFormVisible]);
  
  const handleImageUrlChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setImageUrl(event.target.value);
  };

  const handleImageFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const file = event.target.files[0];
      setImageFile(file);

      const reader = new FileReader();
      reader.onloadend = () => {
        setImageUrl(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };
  
  const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setText(event.target.value);
  };

  const handleSubmitPost = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const newPost: Post = {
      id: Date.now(),
      imageUrl: imageFile ? URL.createObjectURL(imageFile) : imageUrl,
      text,
    };
    setPosts([...posts, newPost]);
    setImageUrl("");
    setImageFile(null);
    setText("");
    setPostFormVisible(false);
  };

  const handleDeletePost = (postId: number) => {
    setPosts(posts.filter((post) => post.id !== postId));
    setActiveMenu(null);
  };

  const handleEditPost = (postId: number) => {
    const post = posts.find((p) => p.id === postId);
    if (post) {
      setImageUrl(post.imageUrl as string);
      setText(post.text);
      setIsEditMode(postId);
      setPostFormVisible(true);
      setActiveMenu(null);
    }
  };

  const handleSaveEdit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setPosts(
        posts.map((post) =>
            post.id === isEditMode
                ? { ...post, imageUrl, text }
                : post
        )
    );
    setImageUrl("");
    setImageFile(null);
    setText("");
    setIsEditMode(null);
    setPostFormVisible(false);
  };

  const toggleMenu = (postId: number) => {
    setActiveMenu((prevActiveMenu) => (prevActiveMenu === postId ? null : postId));
  };
  
  return (
      <div>
        <h2>Ecology Chat Page</h2>
        <div className="add-post">
          <button onClick={togglePostForm} className="open-modal-btn">
            Add Post
          </button>
          {isPostFormVisible && (
              <div className="post-form">
                <form onSubmit={isEditMode === null ? handleSubmitPost : handleSaveEdit}>
                  <div>
                    <label htmlFor="imageUrl">Enter image URL:</label>
                    <input
                        type="text"
                        id="imageUrl"
                        name="Url"
                        value={imageUrl as string}
                        onChange={handleImageUrlChange}
                    />
                  </div>
                  <div>
                    <label htmlFor="imageFile">Upload Image</label>
                    <input
                        type="file"
                        id="imageFile"
                        name="ImageFile"
                        onChange={handleImageFileChange}
                    />
                  </div>
                  <div>
                    <label htmlFor="imageText">Enter text:</label>
                    <input
                        type="text"
                        id="imageText"
                        name="Text"
                        value={text}
                        onChange={handleTextChange}
                    />
                  </div>
                  <button type="submit">{isEditMode === null ? "Add" : "Save"}</button>
                </form>
              </div>
          )}
        </div>

        <ul>
          {posts.map((post) => (
              <li key={post.id} className="post">
                <div className="post-header">
                  {post.imageUrl && (
                      <img src={post.imageUrl as string} alt="Post" className="post-image" />
                  )}
                  <div className="post-actions">
                    <button onClick={() => toggleMenu(post.id)} className="dots">•••</button>
                    {activeMenu === post.id && (
                        <div className="actions-menu">
                          <button onClick={() => handleEditPost(post.id)}>Edit</button>
                          <button onClick={() => handleDeletePost(post.id)}>Delete</button>
                        </div>
                    )}
                  </div>
                </div>
                <p>{post.text}</p>
              </li>
          ))}
        </ul>
      </div>
  );
}

export default EcologyChat;
